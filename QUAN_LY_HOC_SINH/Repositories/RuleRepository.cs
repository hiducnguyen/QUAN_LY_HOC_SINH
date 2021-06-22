using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RuleRepository : IRuleRepository
    {
        private IUnitOfWork _unitOfWork;
        public RuleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IList<Rule> FindAllRules()
        {
            using (_unitOfWork.Start())
            {
                return _unitOfWork.Session.QueryOver<Rule>().List();
            }
        }
        public Rule FindRuleById(int id)
        {
            using (_unitOfWork.Start())
            {
                return _unitOfWork.Session.QueryOver<Rule>()
                    .Where(x => x.Id == id)
                    .SingleOrDefault();
            }
        }
    }
}
