using Resources;
using Repositories.Enums;
using Services;
using Services.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class StudentController : BaseController
    {
        private IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student/Index
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách học sinh";

            IList<IndexStudentDTO> model = _studentService.FindAllStudents();
            
            return View(model);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.Title = Resource.CreateStudent;
            CreateStudentDTO model = new CreateStudentDTO
            {
                AllGenders = CreateSelectListGender(),
                EditMode = false
            };
            return View(model);
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "StudentId,Name,BirthDate,Email,Address,Gender")]
            CreateStudentDTO model)
        {
            if (ModelState.IsValid)
            {
                _studentService.CreateStudent(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Title = Resource.CreateStudent;
                model.AllGenders = CreateSelectListGender();
                model.EditMode = false;
                return View(model);
            }
        }

        // GET: Student/Edit/id
        public ActionResult Edit(int id)
        {
            CreateStudentDTO model = _studentService.FindStudentByStudentId(id);
            model.EditMode = true;
            model.AllGenders = CreateSelectListGender();
            ViewBag.Title = Resource.EditStudent;

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            { 
                int studentId = (int)id;
                _studentService.DeleteStudent(studentId);
            }

            return RedirectToAction("Index");
        }

        public JsonResult IsStudentIdAlreadyExist(int studentId, bool editMode)
        {
            if (editMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if (_studentService.IsStudentIdAlreadyExist(studentId))
            {
                return Json(string.Format(Resource.StudentIdAlreadyExists, studentId), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        private SelectList CreateSelectListGender()
        {
            IEnumerable<SelectListItem> genders = new List<SelectListItem>(GenderHelper.GetAllGenders()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = GenderHelper.GetText(s)
                }));

            genders = genders.OrderBy(g => GenderHelper.ToGender(g.Value));

            return new SelectList(genders, "Value", "Text");
        }
    }
}