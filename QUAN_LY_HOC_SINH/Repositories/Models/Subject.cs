using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class Subject
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ISet<Transcript> Transcripts { get; set; }
        public virtual int Version { get; set; }
    }
}