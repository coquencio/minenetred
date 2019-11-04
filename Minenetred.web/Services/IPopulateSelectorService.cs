using Minenetred.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface IPopulateSelectorService
    {
        Task<List<ActivityDto>> GetActivitiesInListAsync(int projectId, string userName);
        Task<List<IssueDto>> GetIssuesInListAsync(int projectId, string username);
    }
}
