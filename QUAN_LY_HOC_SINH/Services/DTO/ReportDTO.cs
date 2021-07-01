using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ReportDTO
    {
        public string ClassName { get; set; }
        public int NumberOfStudentsInClass { get; set; }
        public int NumberOfStudentsPass { get; set; }
        public string Ratio { get; set; }
    }
}
