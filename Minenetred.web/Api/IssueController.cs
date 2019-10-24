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
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;

        public IssueController(
            IIssueService issueService
            )
        {
            _issueService = issueService;
        }

        [HttpGet]
        [Route("/Issues/{projectId}")]
        [Produces("application/json")]
        [ProducesResponseType(404)]
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}