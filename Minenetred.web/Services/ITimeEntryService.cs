using Minenetred.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface ITimeEntryService
    {
        Task<float> GetTimeEntryHoursPerDay(int projectId, string user, string date = null);
        Task<HttpStatusCode> AddTimeEntryAsync(JObject entry);

        Task<Dictionary<String, int>> GetUnloggedDaysAsync(int UserId, string authKey, DateTime today);
        Task<List<TimeEntryDto>> GetTimeEntriesAsync(string userName, int projectId = 0, string fromDate = null, string toDate = null);
    }
}