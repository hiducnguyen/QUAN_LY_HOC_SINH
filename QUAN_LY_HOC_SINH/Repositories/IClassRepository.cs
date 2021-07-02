using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IClassRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Class FindClassByName(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Class FindClassById(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Class> FindAllClasses();
    }
}
