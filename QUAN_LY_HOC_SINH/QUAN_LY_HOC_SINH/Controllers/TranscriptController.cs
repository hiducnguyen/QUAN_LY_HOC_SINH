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
            IList<SearchTranscriptDTO> model = _transcriptService
                .GetListSearchTranscriptDTOFromAllTranscripts();
            ViewBag.Title = Resource.SearchTranscript;
            return View(model);
        }
    }
}