using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class Class
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Student> Students { get; set; }
        public virtual int NumberOfStudents { get => Students.Count; }
        public virtual int Version { get; set; }
    }
}