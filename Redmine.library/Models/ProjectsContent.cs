using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    // todo: rename to ProjectListResponse
    public class ProjectsContent

    {
        public List<Project> Projects { get; set; }
        // todo: it appears this properties will appear in different resources (projects, issues, time entries, etc)
        // so it would make sense to move them to an abstract class named something like ResultContent and inherit it.
        // todo: keep consistency in naming. Rename to TotalCount instead. 
        public int Total_count { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

    }
}
