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
    public class SubjectController : BaseController
    {
        private ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: Subject
        public ActionResult Index()
        {
            IList<IndexSubjectDTO> model = _subjectService.FindAllSubjects();
            ViewBag.Title = Resource.ListSubjects;
            return View(model);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.Title = Resource.CreateSubject;
            CreateSubjectDTO model = new CreateSubjectDTO();
            return View(model);
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "SubjectId,Name")]
            CreateSubjectDTO model)
        {
            if (ModelState.IsValid)
            {
                _subjectService.CreateSubject(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Title = Resource.CreateSubject;
                return View(model);
            }
        }


        // GET: Subject/Edit/id
        public ActionResult Edit(int id)
        {
            CreateSubjectDTO model = _subjectService.FindSubjectBySubjectId(id);
            model.EditMode = true;
            ViewBag.Title = Resource.EditSubject;

            return View("Create", model);
        }

        // POST: Subject/Edit/id
        [HttpPost]
        public ActionResult Edit(
            [Bind(Include = "SubjectId,Name,Version")]
            CreateSubjectDTO model)
        {
            if (ModelState.IsValid)
            {
                _subjectService.UpdateSubject(model);
                return RedirectToAction("Index");
            }
            else
            {
                model.EditMode = true;
                ViewBag.Title = Resource.EditSubject;

                return View("Create", model);
            }
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                int subjectId = (int)id;
                _subjectService.DeleteSubject(subjectId);
            }

            return RedirectToAction("Index");
        }
        public JsonResult IsSubjectIdAlreadyExist(int subjectId, bool editMode)
        {
            if (editMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if (_subjectService.IsSubjectIdAlreadyExist(subjectId))
            {
                return Json(string.Format(Resource.ObjectAlreadyExists, Resource.Subject, Resource.Id, subjectId)
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsSubjectNameAlreadyExist(string name, bool editMode)
        {
            if (editMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_subjectService.IsSubjectNameAlreadyExist(name))
                {
                    return Json(string.Format(Resource.ObjectAlreadyExists, Resource.Subject, Resource.Name, name)
                        , JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}