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
    public class ClassController : BaseController
    {
        private IClassService _classService;
        private IStudentService _studentService;

        public ClassController(IClassService classService, IStudentService studentService)
        {
            _classService = classService;
            _studentService = studentService;
        }

        // GET: Class
        public ActionResult Index()
        {
            IList<IndexClassDTO> model = _classService.FindAllClasses();
            ViewBag.Title = Resource.ListClasses;
            return View(model);
        }

        // GET: Class/Create
        public ActionResult Create()
        {
            CreateClassDTO model = new CreateClassDTO {
                AllGrades = _classService.GetAllGrades(),
                AllAvaibleStudents = _studentService.GetAllAvailableStudents()
            };
            ViewBag.Title = Resource.CreateClass;
            return View(model);
        }

        // POST: Class/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,Grade,Students")]
            CreateClassDTO model)
        {
            if (ModelState.IsValid)
            {
                _classService.CreateClass(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Title = Resource.CreateClass;
                model.AllGrades = _classService.GetAllGrades();
                model.AllAvaibleStudents = _studentService.GetAllAvailableStudents();
                return View(model);
            }
        }

        [Route("class/edit/{className}")]
        public ActionResult Edit(string className)
        {
            CreateClassDTO model = _classService.FindClassByName(className);
            model.EditMode = true;
            model.AllGrades = _classService.GetAllGrades();
            model.AllAvaibleStudents = _studentService.GetAllAvailableStudents(className);
            ViewBag.Title = Resource.EditClass;
            return View("Create", model);
        }

        [HttpPost]
        [Route("class/edit/{className}")]
        public ActionResult Edit(
            [Bind(Include = "Name,Grade,Students,Version")]
            CreateClassDTO model)
        {
            if (ModelState.IsValid)
            {
                _classService.UpdateClass(model);
                return RedirectToAction("Index");
            }
            else
            {
                model.EditMode = true;
                model.AllGrades = _classService.GetAllGrades();
                model.AllAvaibleStudents = _studentService.GetAllAvailableStudents(model.Name);
                ViewBag.Title = Resource.EditClass;

                return View("Create", model);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (id != null)
            {
                _classService.DeleteClass(id);
            }

            return RedirectToAction("Index");
        }

        public JsonResult IsClassNameTaken(string name, bool editMode)
        {
            if (editMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            if (_classService.IsClassNameExist(name))
            {
                return Json(string.Format(Resource.ObjectAlreadyExists, Resource.Class, Resource.Name, name)
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}