using System;

namespace Repositories.Models
{
    public class Rule
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
        public virtual int Version { get; set; }
    }
}