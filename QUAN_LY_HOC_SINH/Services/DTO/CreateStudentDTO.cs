using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Services.DTO
{
    public class CreateStudentDTO
    {
        [Display(Name = "StudentId", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [Range(1000, 9999, ErrorMessageResourceName = "RangeError", ErrorMessageResourceType = typeof(Resource))]
        [Remote("IsStudentIdAlreadyExist", "Student", AdditionalFields = "EditMode")]
        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true)]
        public int StudentId { get; set; }

        [Display(Name = "StudentName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(255, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Display(Name = "BirthDate", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "EmailAddressError", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(255, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Gender { get; set; }

        public SelectList AllGenders { get; set; }
        public bool EditMode { get; set; }
    }
}
