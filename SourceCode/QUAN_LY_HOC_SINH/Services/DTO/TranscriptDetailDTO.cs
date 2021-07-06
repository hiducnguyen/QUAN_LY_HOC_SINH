using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class TranscriptDetailDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        [Display(Name = "FifteenMinutesTestScore", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Range(0, 10, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float FifteenMinutesTestScore { get; set; }

        [Display(Name = "FortyFiveMinutesTestScore", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Range(0, 10, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float FortyFiveMinutesTestScore { get; set; }

        [Display(Name = "FinalTestScore", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Range(0, 10, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float FinalTestScore { get; set; }
        public int Version { get; set; }
    }
}
