using Repositories.Enums;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITranscriptRepository
    {
        /// <summary>
        /// Find all transcripts
        /// </summary>
        /// <returns></returns>
        IList<Transcript> FindAllTranscripts();
        /// <summary>
        /// Find all transcripts which belong to a subject with corresponding subjectId
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        IList<Transcript> FindAllTranscripts(Subject subject);
        /// <summary>
        /// Find all transcripts in semester which belong to a student with corresponding studentId
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="semester"></param>
        /// <returns></returns>
        IList<Transcript> FindAllTranscripts(Guid studentId, Semester semester);
        /// <summary>
        /// Find all class and corresponding transcript of each semester
        /// </summary>
        /// <returns></returns>
        IList<TranscriptOfClass> FindAllTranscriptsOfClass();
        /// <summary>
        /// Find class and corresponding transcript of semester
        /// </summary>
        /// <param name="className"></param>
        /// <param name="subjectId"></param>
        /// <param name="Semester"></param>
        /// <returns></returns>
        TranscriptOfClass FindTranscriptsOfClass(string className, int subjectId, Semester Semester);

        /// <summary>
        /// Find transcript of student in corresponding subject and semester
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="studentId"></param>
        /// <param name="semester"></param>
        /// <returns></returns>
        Transcript FindTranscript(Subject subject, Guid studentId, Semester semester);
    }
}
