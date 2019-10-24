using System;

namespace Redmine.Library.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public Issue Issue { get; set; }
        public Activity Activity { get; set; }
        public float Hours { get; set; }
        public string Comments { get; set; }
        public DateTime SpentOn { get; set; }
    }
}