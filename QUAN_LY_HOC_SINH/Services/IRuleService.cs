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
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Maximum age of student which is allowed when they are added</returns>
        int GetMaximumAge();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Minimum age of student which is allowed when they are added</returns>
        int GetMinimumAge();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Maximum number of student which is allowed in a class when they are created</returns>
        int GetMaxNumberOfStudentEachClass();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The minimum score a student need to have to pass a subject</returns>
        float GetMinimumPassScored();
    }
}
