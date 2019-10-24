using Minenetred.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface IActivityService
    {
        Task<List<ActivityDto>> GetActivitiesAsync(int projectId, string email);
    }
}