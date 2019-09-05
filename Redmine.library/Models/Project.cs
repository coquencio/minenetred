using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public int status { get; set; }
        public DateTime Created_on { get; set; }
        public DateTime Updated_on { get; set; }
    }
}
