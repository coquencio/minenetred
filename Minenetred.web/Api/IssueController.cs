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
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly ILogger<IssueController> _logger;

        public IssueController(
            IIssueService issueService,
            ILogger<IssueController> logger
            )
        {
            _issueService = issueService;
            _logger = logger;
        }

        [HttpGet]
        [Route("/Issues/{projectId}")]
        [Produces("application/json")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<List<IssueDto>>> GetIssuesAsync([FromRoute] int projectId)
        {
            try
            {
                var toReturn = await _issueService.GetIssuesAsync(projectId, UserPrincipal.Current.EmailAddress);
                if (toReturn == null)
                {
                    return NotFound();
                }
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
    }
}