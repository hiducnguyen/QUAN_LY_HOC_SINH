using Repositories.Enums;
using System;

namespace Repositories.Models
{
    public class Transcript
    {
        public virtual Guid Id { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual float FifteenMinuteTestScore { get; set; }
        public virtual float FortyFiveMinuteTestScore { get; set; }
        public virtual float FinalTestScore { get; set; }
        public virtual float AverageScore { get => (FifteenMinuteTestScore + FortyFiveMinuteTestScore * 2 + FinalTestScore * 3) / 6; }
        public virtual int Version { get; set; }
    }
}