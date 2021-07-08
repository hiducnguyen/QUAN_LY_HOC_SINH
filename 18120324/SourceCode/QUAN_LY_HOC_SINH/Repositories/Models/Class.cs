using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class Class
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Grade { get; set; }
        public virtual ISet<Student> Students { get; set; }
        public virtual int Version { get; set; }
    }
}