using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services
{
    public interface IIssueService
    {
        Task<IssueListResponse> GetIssuesAsync(string authKey, int assignedToId, int projectId);
    }
}
