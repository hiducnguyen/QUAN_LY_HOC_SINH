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
        private IStudentService _studentService;

        public TranscriptController(ITranscriptService scriptService,
            IClassService classService,
            ISubjectService subjectService,
            IStudentService studentService)
        {
            _transcriptService = scriptService;
            _classService = classService;
            _subjectService = subjectService;
            _studentService = studentService;
        }

        // GET: Transcript
        public ActionResult Index()
        {
            IList<IndexTranscriptDTO> model = _transcriptService.GetListIndexTranscriptDTOFromAllTranscripts();
            ViewBag.Title = Resource.ListScripts;
            return View(model);
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

        // GET: Transcript/Search
        public ActionResult Search()
        {
            ViewBag.Title = Resource.SearchTranscript;
            ViewBag.AllStudents = _studentService.GetSelectListOfAllStudents();
            return View();
        }

        // POST: Transcript/Search
        [HttpPost]
        public ActionResult Search(int studentId)
        {
            SearchTranscriptDTO model = _transcriptService.GetSearchTranscriptDTOByStudentId(studentId);
            ViewBag.Title = Resource.SearchTranscript;
            ViewBag.AllStudents = _studentService.GetSelectListOfAllStudents();
            return View(model);
        }
    }
}