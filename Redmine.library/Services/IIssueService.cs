using Redmine.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redmine.Library.Services
{
    public interface IIssueService
    {
        Task<List<Issue>> GetIssuesAsync(string authKey, int assignedToId, int projectId);
    }
}