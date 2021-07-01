using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services
{
    public interface ITranscriptService
    {
        /// <summary>
        /// Find all scripts and map each of them to IndexTranscriptDTO
        /// </summary>
        /// <returns></returns>
        IList<IndexTranscriptDTO> GetListIndexTranscriptDTOFromAllTranscripts();
        /// <summary>
        /// Find all scripts and map each of them to SearchTranscriptDTO
        /// </summary>
        /// <returns></returns>
        IList<SearchTranscriptDTO> GetListSearchTranscriptDTOFromAllTranscripts();
        /// <summary>
        /// Create transcripts base on DTO
        /// </summary>
        /// <param name="createTranscriptDTO"></param>
        void CreateTranscript(CreateTranscriptDTO createTranscriptDTO);
        /// <summary>
        /// Delete all transcript of corresponding class, subject, and semester
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="className"></param>
        /// <param name="semester"></param>
        void DeleteTranscripts(int subjectId, string className, int semester);
        /// <summary>
        /// Find all transcripts of corresponding subject, class and semester, transfert to DTO
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="className"></param>
        /// <param name="semester"></param>
        /// <returns></returns>
        IList<TranscriptDetailDTO> FindTranscripts(int subjectId, string className, int semester);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="semester"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        IList<ReportDTO> FindSubjectReports(int semester, int subjectId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        IList<ReportDTO> FindSemesterReports(int semester);
        /// <summary>
        /// Update the scripts of corresponding subject, semester and student
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="className"></param>
        /// <param name="semester"></param>
        /// <param name="transcriptDetailDTOs"></param>
        void UpdateTranscripts(int subjectId, string className, int semester,
            IList<TranscriptDetailDTO> transcriptDetailDTOs);
    }
}
