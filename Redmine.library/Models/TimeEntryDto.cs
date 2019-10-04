using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    public class TimeEntryDto
    {
        public int IssueId { get; set; }
        public string SpentOn{ get; set; }
        public float Hours { get; set; }
        public int ActivityId { get; set; }
        public string Comments { get; set; }
    }
}
