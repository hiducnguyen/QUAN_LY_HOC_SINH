using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ISubjectRepository
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        IList<Subject> FindAllSubjects();
        /// <summary>
        /// Find subject by SubjectId
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        Subject FindSubjectBySubjectId(int subjectId);
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Subject FindSubjectByName(string name);
    }
}
