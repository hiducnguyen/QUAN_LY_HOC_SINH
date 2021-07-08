using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class TranscriptOfClass
    {
        virtual public string ClassName { get; set; }
        virtual public Semester Semester { get; set; }
        virtual public int SubjectId { get; set; }
        virtual public string SubjectName { get; set; }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
