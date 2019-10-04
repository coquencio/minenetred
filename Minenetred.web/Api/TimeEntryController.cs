using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
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
    public class TimeEntryController : Controller
    {
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntryController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        [Route("/Entries/{projectId}/{date}")]
        [Produces("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        [HttpGet]
        public async Task<ActionResult<float>> GetTimeSpentPerDate([FromRoute] int projectId, [FromRoute]  string date)
        {
            var toReturn = await _timeEntryService.GetTimeEntryHoursPerDay(projectId, date);
            return Ok(toReturn);
        }

        [Route("/Entries")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<HttpStatusCode> AddTimeEntryAsync([FromBody]TimeEntryFormDto entry)
        {
            if (entry == null)
            {
                return HttpStatusCode.BadRequest;
            }
            return await _timeEntryService.AddTimeEntryAsync(entry);
        }
    }
}