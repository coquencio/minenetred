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
using Redmine.library.Services;
using Microsoft.AspNetCore.Authorization;
using Minenetred.web.Context;
using Minenetred.web.Context.ContextModels;
using Minenetred.web.Infrastructure;
using System.DirectoryServices.AccountManagement;

namespace Minenetred.web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly MinenetredContext _context;
        private readonly IEncryptionService _encryptionService;

        public ProjectsController(
            IMapper mapper,
            IProjectService service,
            MinenetredContext context,
            IEncryptionService encryptionService
            )
        {
            _mapper = mapper;
            _projectService = service;
            _context = context;
            _encryptionService = encryptionService;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        [Route("/")]
        [HttpGet]
        public async Task<ActionResult<ProjectsViewModel>> GetProjectsAsync()
        {
            var userName = UserPrincipal.Current.EmailAddress;
            var user = _context.Users.SingleOrDefault(c => c.UserName == userName);
            if (user == null)
            {
                var newUser = new User()
                {
                    UserName = userName,
                    CreatedDate = DateTime.Now,

                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                user = _context.Users.SingleOrDefault(c => c.UserName == userName);
            }

            if (user.RedmineKey == null)
            {
                return RedirectToAction("AddKey");
            }

            var decryptedKey = _encryptionService.Decrypt(user.RedmineKey);
            var apiContent = await _projectService.GetProjectsAsync(decryptedKey);
            var projectsList = _mapper.Map<ProjectListResponse, ProjectsViewModel>(apiContent);
            var shapedList = new ProjectsViewModel()
            {
                Projects = new List<ProjectDto>(),
            };
            foreach (var project in projectsList.Projects)
            {
                if (project.status == 1)
                    shapedList.Projects.Add(project);
            }
            return View(shapedList);
        }
        [Route("/AccessKey")]
        public IActionResult AddKey()
        {
            var userName = UserPrincipal.Current.EmailAddress;
            ViewBag.user = userName;

            var userKey = _context.Users.SingleOrDefault(c => c.UserName == userName).RedmineKey;
            if (userKey == null)
            {
                ViewBag.key = null;
            }
            else
            { 
                var denryptionKey = _encryptionService.Decrypt(userKey);
                ViewBag.key = denryptionKey;
            }
            return View();
        } 

        [HttpPost]
        public IActionResult KeyUpdate(string key)
        {
            if (string.IsNullOrEmpty(key))
                return RedirectToAction("AddKey");

            var encryptedKey = _encryptionService.Encrypt(key);
            var userName = UserPrincipal.Current.EmailAddress;
            var user = _context.Users.SingleOrDefault(c => c.UserName == userName);
            user.RedmineKey = encryptedKey;
            user.LastKeyUpdatedDate = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("GetProjectsAsync");
        }
    }
}
