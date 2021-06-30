using Repositories.Enums;
using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TranscriptRepository : ITranscriptRepository
    {
        private IUnitOfWork _unitOfWork;

        public TranscriptRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Transcript> FindAllTranscripts(Subject subject)
        {
            return _unitOfWork.Session.QueryOver<Transcript>()
                .Where(x => x.Subject == subject).List();
        }

        public IList<TranscriptOfClass> FindAllTranscriptsOfClass()
        {
            return _unitOfWork.Session.QueryOver<TranscriptOfClass>().List();
        }

        public Transcript FindTranscript(Subject subject, Guid studentId, Semester semester)
        {
            return _unitOfWork.Session.QueryOver<Transcript>()
                .Where(x => x.Subject == subject &&
                    x.StudentId == studentId &&
                    x.Semester == semester)
                .SingleOrDefault();
        }

        public TranscriptOfClass FindTranscriptsOfClass(string className, int subjectId, Semester Semester)
        {
            return _unitOfWork.Session.QueryOver<TranscriptOfClass>()
                .Where(x => x.ClassName == className &&
                    x.SubjectId == subjectId &&
                    x.Semester == Semester)
                .SingleOrDefault();
        }
    }
}
