using System;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Mvc;
using Minenetred.web.Services;

namespace Minenetred.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersManagementService _usersManagementService;
        public AccountController(IUsersManagementService usersManagementService)
        {
            _usersManagementService = usersManagementService;
        }

        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("Needed windows auth");

            _usersManagementService.RegisterUser(UserPrincipal.Current.EmailAddress);
            return RedirectToAction("GetProjectsAsync", "Projects");
        }
    }
}