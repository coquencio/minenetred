using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Redmine.library.Core;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Net;
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

        public async Task<HttpStatusCode> AddTimeEntryAsync (TimeEntryDtoContainer entry, string authKey)
        {
            try
            {
                if (entry == null)
                {
                    throw new Exception("Time entry is null");
                }
                if (string.IsNullOrEmpty(authKey))
                {
                    throw new ArgumentNullException(Constants.nullKeyException);
                }
                var requestUri = Constants.timeEntries +
                    Constants.json +
                    "?key=" + authKey;

                var json = JsonConvert.SerializeObject(entry, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented
                });
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage  response = await _client.PostAsync(requestUri, httpContent);
                return response.StatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
