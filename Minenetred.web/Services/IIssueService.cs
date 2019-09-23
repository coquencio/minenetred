using Minenetred.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Services
{
    public interface IIssueService
    {
        Task<IssueViewModel> GetIssuesAsync(int projectId);
    }
}
