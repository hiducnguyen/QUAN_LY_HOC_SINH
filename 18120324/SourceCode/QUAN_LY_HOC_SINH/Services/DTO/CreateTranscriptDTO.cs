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
    public class CreateTranscriptDTO
    {
        [Display(Name = "Class", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string ClassName { get; set; }

        public SelectList AllClasses { get; set; }

        [Display(Name = "Subject", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int SubjectId { get; set; }

        public SelectList AllSubjects { get; set; }

        [Display(Name = "Semester", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int Semester { get; set; }

        public SelectList AllSemesters { get; set; }

        public bool EditMode { get; set; }

        public int Version { get; set; }
    }
}
