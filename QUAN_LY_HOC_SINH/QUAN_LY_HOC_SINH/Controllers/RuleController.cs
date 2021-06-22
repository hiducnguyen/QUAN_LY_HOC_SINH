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
    }
}