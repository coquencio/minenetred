using Microsoft.AspNetCore.Mvc;
using Minenetred.Web.Services;
using System.DirectoryServices.AccountManagement;

namespace Minenetred.Web.Controllers
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