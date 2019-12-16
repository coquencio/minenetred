﻿using System;
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
            try
            {
                var userEmail = UserPrincipal.Current.EmailAddress;
                if (!_usersManagementService.HasRedmineAddress(userEmail))
                {
                    throw new ArgumentNullException("Base address", "Missing base address");
                }
                if(!await _usersManagementService.IsValidBaseAddressAsync()){
                    throw new InvalidCastException("Invalid base address");
                }
                if (_usersManagementService.HasRedmineKey(userEmail))
                {
                    throw new ArgumentNullException("Api key", "Missing api key");
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
                _logger.LogError(ex, "Invalid api key");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Missing data");
            }
            catch (InvalidCastException ex)
            {
                _logger.LogError(ex, "Invalid base address");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            return BadRequest();
        }
    }
}