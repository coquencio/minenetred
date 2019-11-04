using Minenetred.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.Web.Services.Implementations
{
    public class PopulateSelectorService : IPopulateSelectorService
    {
        private readonly IActivityService _activityService;
        private readonly IIssueService _issueService;
        public PopulateSelectorService(
            IActivityService activityService,
            IIssueService issueService)
        {
            _activityService = activityService;
            _issueService = issueService;
        }

        public async Task<List<ActivityDto>> GetActivitiesInListAsync(int projectId, string userName)
        {
            var toReturn = new List<ActivityDto>();
            var activities = await _activityService.GetActivitiesAsync(projectId, userName);
            foreach (var activity in activities)
            {
                toReturn.Add(activity);
            }
            return toReturn;
        }

        public async Task<List<IssueDto>> GetIssuesInListAsync(int projectId, string username)
        {
            var toReturn = new List<IssueDto>();
            var issues = await _issueService.GetIssuesAsync(projectId, username);
            foreach (var issue in issues)
            {
                toReturn.Add(issue);
            }
            return toReturn;
        }
    }
}
