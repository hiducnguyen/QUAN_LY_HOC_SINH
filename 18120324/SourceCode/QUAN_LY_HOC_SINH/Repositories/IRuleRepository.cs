using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRuleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All rule in database</returns>
        IList<Rule> FindAllRules();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Rule with the corresponding id or null (if rule not found)</returns>
        Rule FindRuleById(int id);
    }
}
