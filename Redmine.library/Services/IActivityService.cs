using Redmine.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redmine.Library.Services
{
    public interface IActivityService
    {
        Task<List<Activity>> GetActivityListResponseAsync(string authKey, int projectId);
    }
}