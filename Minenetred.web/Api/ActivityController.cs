using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minenetred.Web.Models;
using Minenetred.Web.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;

namespace Minenetred.Web.Api
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [Produces("application/json")]
        [ProducesResponseType(404)]
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}