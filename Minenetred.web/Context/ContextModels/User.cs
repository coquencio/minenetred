using System;
using System.ComponentModel.DataAnnotations;

namespace Minenetred.Web.Context.ContextModels
{
    public class User
    {
        public Guid UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string RedmineKey { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime LastKeyUpdatedDate { get; set; }
        public int RedmineId { get; set; }
    }
}