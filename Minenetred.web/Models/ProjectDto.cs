using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Models
{
    public class ProjectDto
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
