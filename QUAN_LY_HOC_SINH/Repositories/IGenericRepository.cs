using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IGenericRepository
    {
        /// <summary>
        /// Save the object to database
        /// </summary>
        /// <param name="o"></param>
        object Save(object o);
        /// <summary>
        /// Delete the object from database
        /// </summary>
        /// <param name="o"></param>
        void Delete(object o);
        /// <summary>
        /// Update the object
        /// </summary>
        /// <param name="o"></param>
        void Update(object o);
    }
}
