using Minenetred.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface IIssueService
    {
        Task<List<IssueDto>> GetIssuesAsync(int projectId, string email);
    }
}