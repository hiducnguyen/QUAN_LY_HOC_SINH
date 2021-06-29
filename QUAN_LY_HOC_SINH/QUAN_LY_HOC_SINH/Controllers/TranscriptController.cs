using Repositories.Enums;
using Resources;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class TranscriptController : BaseController
    {
        private ITranscriptService _transcriptService;
        private IClassService _classService;
        private ISubjectService _subjectService;
        public TranscriptController(ITranscriptService scriptService,
            IClassService classService,
            ISubjectService subjectService)
        {
            _transcriptService = scriptService;
            _classService = classService;
            _subjectService = subjectService;
        }

        // GET: Transcript
        public ActionResult Index()
        {
            IList<IndexTranscriptDTO> model = _transcriptService.FindAllTranscripts();
            ViewBag.Title = Resource.ListScripts;
            return View(model);
        }

        // GET: Transcript/Create
        public ActionResult Create()
        {
            CreateTranscriptDTO model = new CreateTranscriptDTO
            {
                AllClasses = _classService.GetSelectListClasses(),
                AllSemesters = SemesterHelper.GetAllSemesters(),
                AllSubjects = _subjectService.GetSelectListSubjects()
            };
            ViewBag.Title = Resource.CreateTranscript;
            return View(model);
        }

        // POST: Transcript/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ClassName,SubjectId,Semester")]
            CreateTranscriptDTO model)
        {
            if (ModelState.IsValid)
            {
                _transcriptService.CreateTranscript(model);
                return RedirectToAction("Detail", new RouteValueDictionary {
                    {"subjectId", model.SubjectId },
                    {"className", model.ClassName },
                    {"semester", model.Semester }
                });
            }
            else
            {
                ViewBag.Title = Resource.CreateTranscript;
                model.AllClasses = _classService.GetSelectListClasses();
                model.AllSemesters = SemesterHelper.GetAllSemesters();
                model.AllSubjects = _subjectService.GetSelectListSubjects();
                return View(model);
            }
        }

        [Route("transcript/detail/{subjectId}/{className}/{semester}")]
        public ActionResult Detail(int subjectId, string className, int semester)
        {
            IList<TranscriptDetailDTO> model = _transcriptService.FindTranscripts(subjectId, className, semester);

            ViewBag.ClassName = className;
            ViewBag.SubjectName = _subjectService.FindSubjectBySubjectId(subjectId).Name;
            ViewBag.Semester = semester;
            ViewBag.Title = Resource.TranscriptDetail;

            return View(model);
        }

        [HttpPost]
        [Route("transcript/detail/{subjectId}/{className}/{semester}")]
        public ActionResult Detail(int subjectId, string className, int semester,
            [Bind(Include = "StudentId,FifteenMinutesTestScore,FortyFiveMinutesTestScore,FinalTestScore,Version")]
            IList<TranscriptDetailDTO> model)
        {
            if (ModelState.IsValid)
            {
                _transcriptService.UpdateTranscripts(subjectId, className, semester, model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ClassName = className;
                ViewBag.SubjectName = _subjectService.FindSubjectBySubjectId(subjectId).Name;
                ViewBag.Semester = semester;
                ViewBag.Title = Resource.TranscriptDetail;

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            // format of id: "{SubjectId}/{ClassName}/{Semester}"
            if (id != null)
            {
                string[] ids = id.Split('/');
                if (ids.Length == 3)
                {
                    _transcriptService.DeleteTranscripts(Convert.ToInt32(ids[0]), ids[1], Convert.ToInt32(ids[2]));
                }
            }

            return RedirectToAction("Index");
        }
    }
}