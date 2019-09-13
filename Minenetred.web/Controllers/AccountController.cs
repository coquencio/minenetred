using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Minenetred.web.Context;
using Minenetred.web.Context.ContextModels;
using Minenetred.web.Infrastructure;

namespace Minenetred.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly MinenetredContext _context;
        public AccountController(MinenetredContext context)
        {
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("Needed windows auth");

            var userMail = UserPrincipal.Current.EmailAddress;
            var user = _context.Users.SingleOrDefault(c=>c.UserName== userMail);
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("GetProjectsAsync", "Projects");
        }
    }
}