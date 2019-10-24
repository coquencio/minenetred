using Redmine.Library.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Redmine.Library.Services
{
    public interface ITimeEntryService
    {
        Task<List<TimeEntry>> GetTimeEntriesAsync(
            string authKey,
            int userId,
            int projectId = 0,
            string fromDate = null,
            string toDate = null);

        Task<HttpStatusCode> AddTimeEntryAsync(string authKey, int issueId, string spentOn, double hours, int activityId, string comments);
    }
}