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
        Task<float> GetTimeEntryHoursPerDay(int projectId, string date, string user);

        Task<HttpStatusCode> AddTimeEntryAsync(JObject entry);

        Task<Dictionary<String, int>> GetUnloggedDaysAsync(int UserId, string authKey, DateTime today);
    }
}