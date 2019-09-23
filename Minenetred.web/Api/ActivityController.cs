using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minenetred.web.Context;
using Minenetred.web.Infrastructure;
using Minenetred.web.Models;
using Minenetred.web.Services;
using Minenetred.web.ViewModels;
using Redmine.library.Models;

namespace Minenetred.web.Api
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }
        [Route("/Activities/{projectId}")]
        [HttpGet]
        public async Task<ActionResult<ActivityViewModel>> GetActivitiesAsync([FromRoute] int projectId)
        {
            try
            {
                var toRetun = await _activityService.GetActivitiesAsync(projectId);
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