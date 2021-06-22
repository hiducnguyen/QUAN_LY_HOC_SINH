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
    public class RuleController : BaseController
    {
        private IRuleService _ruleService;
        public RuleController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        // GET: Rule
        public ActionResult Index()
        {
            IList<UpdateRuleDTO> model = _ruleService.FindAllRules();
            ViewBag.Title = Resource.ListRules;
            return View(model);
        }

        //  POST: Rule/Edit/Id
        [HttpPost]
        public ActionResult Edit(int id, string value, int version)
        {
            UpdateRuleDTO updateRuleDTO = new UpdateRuleDTO
            {
                Id = id,
                Value = value,
                Version = version
            };
            _ruleService.UpdateRule(updateRuleDTO);
            return RedirectToAction("Index");
        }

        public JsonResult IsStudentAgeValid(DateTime birthDate, bool editMode)
        {
            // If a student already created, we will not validate their age anymore.
            if (editMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            // Check if a student age is match the rule or not.
            int age = CalculateAge(birthDate, DateTime.Now);
            int minimumAge = _ruleService.GetMinimumAge();
            int maximumAge = _ruleService.GetMaximumAge();
            
            if (age > maximumAge || minimumAge > age)
            {
                return Json(string.Format(Resource.StudentAgeInvalid, minimumAge, maximumAge)
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        private int CalculateAge(DateTime birthDate, DateTime currentDate)
        {
            int age = currentDate.Year - birthDate.Year;
            
            if (currentDate.Month < birthDate.Month ||
                (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }
    }
}