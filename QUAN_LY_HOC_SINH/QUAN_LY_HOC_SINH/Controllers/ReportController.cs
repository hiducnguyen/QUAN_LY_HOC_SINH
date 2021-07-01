using Repositories.Enums;
using Resources;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class ReportController : Controller
    {
        private ITranscriptService _transcriptService;
        private ISubjectService _subjectService;

        public ReportController(ITranscriptService transcriptService,
            ISubjectService subjectService)
        {
            _transcriptService = transcriptService;
            _subjectService = subjectService;
        }

        // GET: Report/Subject
        public ActionResult Subject()
        {
            ViewBag.Title = Resource.CreateSubjectReport;
            ViewBag.AllSubjects = _subjectService.GetSelectListSubjects();
            ViewBag.AllSemesters = SemesterHelper.GetSelectListOfAllSemesters();
            return View();
        }

        // POST: Report/Subject
        [HttpPost]
        public ActionResult Subject(int semester, int subjectId)
        {
            IList<ReportDTO> model = _transcriptService.FindSubjectReports(semester, subjectId);
            ViewBag.Title = Resource.SubjectReport;
            ViewBag.Semester = semester;
            ViewBag.Subject = _subjectService.FindSubjectBySubjectId(subjectId).Name;
            return View("SubjectReport", model);
        }

        // GET: Report/Semester
        public ActionResult Semester()
        {
            ViewBag.Title = Resource.CreateSemesterReport;
            ViewBag.AllSemesters = SemesterHelper.GetSelectListOfAllSemesters();
            return View();
        }

        // POST: Report/Semester
        [HttpPost]
        public ActionResult Semester(int semester)
        {
            IList<ReportDTO> model = _transcriptService.FindSemesterReports(semester);
            ViewBag.Title = Resource.SubjectReport;
            ViewBag.Semester = semester;
            return View("SemesterReport", model);
        }
    }
}