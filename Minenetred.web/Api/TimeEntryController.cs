using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minenetred.Web.Models;
using Minenetred.Web.Services;
using Newtonsoft.Json.Linq;

namespace Minenetred.Web.Api
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
            var toReturn = await _timeEntryService.GetTimeEntryHoursPerDay(projectId, date, UserPrincipal.Current.EmailAddress);
            return Ok(toReturn);
        }

        [Route("/Entries")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<HttpStatusCode> AddTimeEntryAsync([FromBody]JObject entry)
        {
            if (entry == null)
            {
                return HttpStatusCode.BadRequest;
            }
            return await _timeEntryService.AddTimeEntryAsync(entry);
        }
    }
}