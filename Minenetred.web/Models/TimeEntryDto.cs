using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Models
{
    public class TimeEntryDto
    {
        public int Id { get; set; }
        public IssueDto Issue { get; set; }
        public ActivityDto Activity { get; set; }
        public float Hours { get; set; }
        public string Comments { get; set; }
        public DateTime SpentOn { get; set; }
    }
}
