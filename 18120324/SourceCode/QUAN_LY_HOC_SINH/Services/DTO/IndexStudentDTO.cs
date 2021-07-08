using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class IndexStudentDTO
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
