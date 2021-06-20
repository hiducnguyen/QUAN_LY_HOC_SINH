using Repositories.Enums;
using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class Student
    {
        public virtual Guid Id { get; set; }
        public virtual int StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string Address { get; set; }
        public virtual string Email { get; set; }
        public virtual ISet<Transcript> Transcripts { get; set; }
        public virtual int Version { get; set; }
    }
}