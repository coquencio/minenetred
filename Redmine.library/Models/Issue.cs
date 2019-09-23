using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}
