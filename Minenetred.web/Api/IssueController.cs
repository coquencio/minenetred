using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minenetred.web.Context;
using Minenetred.web.Infrastructure;
using Minenetred.web.Services;
using Minenetred.web.ViewModels;
using Redmine.library.Models;

namespace Minenetred.web.Api
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
        public async Task<ActionResult<IssueViewModel>> GetIssuesAsync([FromRoute] int projectId)
        {
            try
            {
                var toReturn = await _issueService.GetIssuesAsync(projectId);
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