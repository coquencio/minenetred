﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minenetred.Web.Services;
using Minenetred.Web.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Minenetred.Web.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUsersManagementService _userManagementService;
        private readonly ITimeEntryService _timeEntryService;
        private readonly IPopulateSelectorService _populateSelectorService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IProjectService service,
            IUsersManagementService userManagementService,
            ITimeEntryService timeEntryService,
            IPopulateSelectorService populateSelectorService,
            ILogger<ProjectsController>  logger
            )
        {
            _projectService = service;
            _userManagementService = userManagementService;
            _timeEntryService = timeEntryService;
            _populateSelectorService = populateSelectorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetProjectsAsync()
        {
            try
            {
                var userName = UserPrincipal.Current.EmailAddress;
                if (!_userManagementService.IsUserRegistered(userName))
                    _userManagementService.RegisterUser(userName);

                if (!_userManagementService.HasRedmineAddress(userName))
                    return RedirectToAction("KeySettingsAsync", "ApiKey", new { msj = "Missing base address" });

                if (!_userManagementService.HasRedmineKey(userName))
                    return RedirectToAction("KeySettingsAsync", "ApiKey", new { msj = "Missing redmine key" });

                var decryptedKey = _userManagementService.GetUserKey(userName);
                var projectList = await _projectService.GetOpenProjectsAsync(decryptedKey);
                var activityDictionary = new Dictionary<int, List<ActivityDto>>();
                var issueDictionary = new Dictionary<int, List<IssueDto>>();

                foreach (var project in projectList)
                {
                    var activitiesToAdd = await _populateSelectorService.GetActivitiesInListAsync(project.Id, userName);
                    var issuesToAdd = await _populateSelectorService.GetIssuesInListAsync(project.Id, userName);

                    activityDictionary.Add(project.Id, activitiesToAdd);
                    issueDictionary.Add(project.Id, issuesToAdd);
                }
                ViewBag.lastAct = activityDictionary.Last();
                ViewBag.lastIss = issueDictionary.Last();
                ViewBag.activities = activityDictionary;
                ViewBag.issues = issueDictionary;
                ViewBag.Warnings = await _timeEntryService.GetUnloggedDaysAsync(_userManagementService.GetRedmineId(userName: userName), decryptedKey, DateTime.Today);
                return View(projectList);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex , "Invalid key");
                return RedirectToAction("KeySettingsAsync", "ApiKey", new { msj = "Add a valid key" });
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError(ex, "Bad request");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex ,"Unhandled exception");
            }
            return BadRequest();
        }
    }
}