using NHibernate;
using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ClassRepository : IClassRepository
    {
        private IUnitOfWork _unitOfWork;

        public ClassRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Class> FindAllClasses()
        {
            return _unitOfWork.Session.QueryOver<Class>().List();
        }

        public Class FindClassById(Guid id)
        {
            return _unitOfWork.Session.QueryOver<Class>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
        }

        public Class FindClassByName(string name)
        {
            Class @class = _unitOfWork.Session.QueryOver<Class>()
                .Where(x => x.Name == name)
                .SingleOrDefault();
            if (@class != null)
            {
                NHibernateUtil.Initialize(@class.Students);
            }
            return @class;
        }
    }
}
