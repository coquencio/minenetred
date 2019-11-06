using System;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minenetred.Web.Services;
using Newtonsoft.Json.Linq;

namespace Minenetred.Web.Api
{
    [Authorize]
    public class TimeEntryController : Controller
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly ILogger<TimeEntryController> _logger;
        public TimeEntryController(ITimeEntryService timeEntryService, 
            ILogger<TimeEntryController> logger)
        {
            _timeEntryService = timeEntryService;
            _logger = logger; 
        }
        [Route("/Entries/{projectId}/{date}")]
        [Produces("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(201)]
        [HttpGet]
        public async Task<ActionResult<float>> GetTimeSpentPerDate([FromRoute] int projectId, [FromRoute]  string date)
        {
            try
            {
                var toReturn = await _timeEntryService.GetTimeEntryHoursPerDay(projectId, date, UserPrincipal.Current.EmailAddress);
                return Ok(toReturn);
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

        [Route("/Entries")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<HttpStatusCode> AddTimeEntryAsync([FromBody]JObject entry)
        {
            if (entry == null)
            {
                _logger.LogError(new ArgumentNullException(),"Time entry empty");
                return HttpStatusCode.BadRequest;
            }
            return await _timeEntryService.AddTimeEntryAsync(entry);
        }
    }
}