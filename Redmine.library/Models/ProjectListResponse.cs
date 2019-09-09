using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    public class ProjectListResponse : ResultContent
    {
        public List<Project> Projects { get; set; }
    }
}
