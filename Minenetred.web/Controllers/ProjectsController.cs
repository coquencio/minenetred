using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redmine.library;
using Minenetred.web.Models;
using Redmine.library.Models;
using Minenetred.web.ViewModels;
using AutoMapper;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Minenetred.web.Context;
using Minenetred.web.Context.ContextModels;
using Minenetred.web.Infrastructure;
using System.DirectoryServices.AccountManagement;
using Minenetred.web.Services;

namespace Minenetred.web.Controllers
{
    [Authorize]
    [ApiExplorerSettings (IgnoreApi =true)]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUsersManagementService _userManagementService;

        public ProjectsController(
            IProjectService service,
            IUsersManagementService userManagementService
            )
        {
            _projectService = service;
            _userManagementService = userManagementService;
        }

        [Route("/")]
        [HttpGet]
        public async Task<ActionResult<ProjectsViewModel>> GetProjectsAsync()
        {
            try
            {
                var userName = UserPrincipal.Current.EmailAddress;
                if (!_userManagementService.IsUserRegistered(userName))
                    _userManagementService.RegisterUser(userName);

                if (!_userManagementService.HasRedmineKey(userName))
                    return RedirectToAction("AddKey");

                var decryptedKey = _userManagementService.GetUserKey(userName);
                var projectList = await _projectService.GetOpenProjectsAsync(decryptedKey);
                return View(projectList);
            }
            catch (Exception)
            {
                return RedirectToAction("AddKey", new {msj = "Add a valid key"});
            }
        }
        [Route("/AccessKey")]
        
        public IActionResult AddKey(string msj = null)
        {
            var userName = UserPrincipal.Current.EmailAddress;
            ViewBag.user = userName;
            if (!string.IsNullOrWhiteSpace(msj))
            {
                ViewBag.msj = msj;
            }
            var userKey = _userManagementService.GetUserKey(userName);
            if (userKey == null)
            {
                ViewBag.key = null;
            }
            else
            {
                var denryptionKey = _userManagementService.GetUserKey(UserPrincipal.Current.EmailAddress);
                ViewBag.key = denryptionKey;
            }
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> UpdateRedmineKeyAsync(string Redminekey)
        {
            try
            {
                if (string.IsNullOrEmpty(Redminekey))
                    return RedirectToAction("AddKey");

                _userManagementService.UpdateKey(Redminekey, UserPrincipal.Current.EmailAddress);
                await _userManagementService.AddRedmineIdAsync(Redminekey, UserPrincipal.Current.EmailAddress);
                return RedirectToAction("GetProjectsAsync");
            }
            catch (Exception)
            {
                return RedirectToAction("AddKey", new {msj="Add a valid key"});
            }
        }
    }
}
