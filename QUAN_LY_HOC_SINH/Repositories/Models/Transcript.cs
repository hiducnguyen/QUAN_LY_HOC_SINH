using Repositories.Enums;
using System;

namespace Repositories.Models
{
    public class Transcript
    {
        public virtual Guid Id { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual float FifteenMinuteTestScore { get; set; }
        public virtual float FortyFiveMinuteTestScore { get; set; }
        public virtual float FinalTestScore { get; set; }
        public virtual int Version { get; set; }
    }
}