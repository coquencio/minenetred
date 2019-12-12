using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minenetred.Web.Models;
using Minenetred.Web.Services;

namespace Minenetred.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IUsersManagementService _usersManagementService;
        private readonly IProjectService _projectService;

        public ProjectsController(
            IUsersManagementService usersManagementService,
            IProjectService projectService
            )
        {
            _usersManagementService = usersManagementService;
            _projectService = projectService;
        }

        [Produces("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<ProjectDto>> GetOpenProjectsAsync()
        {
            var userEmail = UserPrincipal.Current.EmailAddress;
            var redmineKey = _usersManagementService.GetUserKey(userEmail);
            return await _projectService.GetOpenProjectsAsync(redmineKey).ConfigureAwait(false);
        }
    }
}