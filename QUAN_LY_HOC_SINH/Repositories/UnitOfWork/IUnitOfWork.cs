using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Return the session which the unit of work control
        /// </summary>
        ISession Session { get; }
        /// <summary>
        /// Start UnitOfWork (Open a new session if CurrentSession is null or is closed, begin Transaction)
        /// </summary>
        /// <returns></returns>
        IUnitOfWork Start();
        /// <summary>
        /// Flush the session and commit the transaction
        /// </summary>
        void Commit();
    }
}
