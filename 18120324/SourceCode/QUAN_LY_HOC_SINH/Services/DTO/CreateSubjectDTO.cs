using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.DTO
{
    public class CreateSubjectDTO
    {
        [Display(Name = "Id", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Range(100, 999, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        [Remote("IsSubjectIdAlreadyExist", "Subject", AdditionalFields = "EditMode")]
        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true)]
        public int SubjectId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(255, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Remote("IsSubjectNameAlreadyExist", "Subject", AdditionalFields = "EditMode")]
        public string Name { get; set; }

        public bool EditMode { get; set; }

        public int Version { get; set; }
    }
}
