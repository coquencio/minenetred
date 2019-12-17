using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IUsersManagementService usersManagementService,
            IProjectService projectService,
            ILogger<ProjectsController> logger
            )
        {
            _usersManagementService = usersManagementService;
            _projectService = projectService;
            _logger = logger;
        }

        [Produces("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<IActionResult> GetOpenProjectsAsync()
        {
            string message="";
            try
            {
                var userEmail = UserPrincipal.Current.EmailAddress;
                if (!_usersManagementService.HasRedmineAddress(userEmail))
                {
                    message = "Missing base address";
                    throw new ArgumentNullException("Base address", message);
                }
                if(!await _usersManagementService.IsValidBaseAddressAsync()){
                    throw new InvalidCastException("Invalid base address");
                }
                if (!_usersManagementService.HasRedmineKey(userEmail))
                {
                    message = "Missing api key";
                    throw new ArgumentNullException("Api key", message);
                }
                if (!await _usersManagementService.IsApiKeyValidAsync(userEmail))
                {
                    throw new FormatException("Invalid api key");
                }
                var redmineKey = _usersManagementService.GetUserKey(userEmail);
                return Ok(await _projectService.GetOpenProjectsAsync(redmineKey).ConfigureAwait(false));
            } 
            catch (FormatException ex)
            {
                message = "Invalid api key";
                _logger.LogError(ex, message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, message);
            }
            catch (InvalidCastException ex)
            {
                message = "Invalid base address";
                _logger.LogError(ex, message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            return BadRequest(message);
        }
    }
}