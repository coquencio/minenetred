using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minenetred.Web.Models;
using Minenetred.Web.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using System.Threading.Tasks;

namespace Minenetred.Web.Api
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(IActivityService activityService,
            ILogger<ActivityController> logger)
        {
            _activityService = activityService;
            _logger = logger;
        }

        [Produces("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(201)]
        [Route("/Activities/{projectId}")]
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetActivitiesAsync([FromRoute] int projectId)
        {
            try
            {
                var toRetun = await _activityService.GetActivitiesAsync(projectId, UserPrincipal.Current.EmailAddress);
                if (toRetun == null)
                {
                    return NotFound();
                }
                return Ok(toRetun);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Invalid access key");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Bad Request");
            }
            return BadRequest();
        }
    }
}