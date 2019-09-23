using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.library.Models
{
    public class UserServiceModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
