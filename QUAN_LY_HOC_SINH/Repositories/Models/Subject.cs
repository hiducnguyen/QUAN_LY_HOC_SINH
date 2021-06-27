using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class Subject
    {
        public virtual Guid Id { get; set; }
        public virtual int SubjectId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Version { get; set; }
    }
}