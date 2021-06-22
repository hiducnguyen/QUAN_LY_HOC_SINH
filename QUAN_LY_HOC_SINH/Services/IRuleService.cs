using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IRuleService
    {
        /// <summary>
        /// Find all rules and convert each of them to UpdateRuleDTO
        /// </summary>
        /// <returns></returns>
        IList<UpdateRuleDTO> FindAllRules();
        /// <summary>
        /// Update the rule corresponding with the DTO
        /// </summary>
        /// <param name="updateRuleDTO"></param>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <exception cref="Services.Exceptions.ObjectHasBeenUpdatedException"></exception>
        /// <exception cref="Services.Exceptions.UnableToCastException">
        /// Throw when the value cannot cast to the type of the rule
        /// </exception>
        /// <remarks>
        /// You just can update only Value property of the rule
        /// </remarks>
        void UpdateRule(UpdateRuleDTO updateRuleDTO);
    }
}
