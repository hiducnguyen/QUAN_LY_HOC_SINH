using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private IUnitOfWork _unitOfWork;

        public SubjectRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Subject> FindAllSubjects()
        {
            return _unitOfWork.Session.QueryOver<Subject>().List();
        }

        public Subject FindSubjectBySubjectId(int subjectId)
        {
            return _unitOfWork.Session.QueryOver<Subject>().Where(x => x.SubjectId == subjectId).SingleOrDefault();
        }

        public Subject FindSubjectByName(string name)
        {
            return _unitOfWork.Session.QueryOver<Subject>().Where(x => x.Name == name).SingleOrDefault();
        }
    }
}
