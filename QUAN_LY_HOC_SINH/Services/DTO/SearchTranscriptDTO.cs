using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class SearchTranscriptDTO
    {
        public string StudentName { get; set; }
        public string ClassName { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float FirstSemesterAverageScore { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float SecondSemesterAverageScore { get; set; }
    }
}
