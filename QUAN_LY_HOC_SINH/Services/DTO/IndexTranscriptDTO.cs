using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class IndexTranscriptDTO
    {
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int Semester { get; set; }
    }
}
