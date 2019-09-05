using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    public class ProjectsContent

    {
        public List<Project> Projects { get; set; }
        public int Total_count { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

    }
}
