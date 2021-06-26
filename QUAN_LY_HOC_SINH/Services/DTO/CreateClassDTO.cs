using Repositories.Models;
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
    public class CreateClassDTO
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        [Remote("IsClassNameTaken", "Class", AdditionalFields = "EditMode")]
        public string Name { get; set; }

        [Display(Name = "Grade", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int Grade { get; set; }

        public SelectList AllGrades { get; set; }

        [Display(Name = "ListStudents", ResourceType = typeof(Resource))]
        public IList<int> Students { get; set; }
        
        public MultiSelectList AllAvaibleStudents { get; set; }
        
        public bool EditMode { get; set; }
        
        public int Version { get; set; }
    }
}
