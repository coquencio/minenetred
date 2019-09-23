using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services
{
    public interface IActivityService
    {
        Task<ActivityListResponse> GetActivityListResponseAsync(string authKey, int projectId);
    }
}
