using Newtonsoft.Json;
using Redmine.library.Core;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services.Implementations
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly HttpClient _client;
        public TimeEntryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<TimeEntryListResponse> GetTimeEntriesAsync(string authKey, int userId, int projectId, string date)
        {
            try
            {
                if (string.IsNullOrEmpty(authKey))
                    throw new ArgumentNullException(Constants.nullKeyException);

                var toReturn = "";
                var requestUri =
                    Constants.timeEntries +
                    Constants.json +
                    "?key=" + authKey +
                    "&" +
                    Constants.projectId +
                    projectId +
                    "&user_id=" +
                    userId +
                    "&from=" +
                    date +
                    "&to=" +
                    date;
                HttpResponseMessage response = await _client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    toReturn = await response.Content.ReadAsStringAsync();
                    var timeEntryListResponse = JsonConvert.DeserializeObject<TimeEntryListResponse>(toReturn);
                    return timeEntryListResponse;
                }
                else
                {
                    var errormsg = await response.Content.ReadAsStringAsync();
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
