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
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float FirstSemesterAverageScore { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float SecondSemesterAverageScore { get; set; }
    }
}
