using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redmine.Library.Core;
using Redmine.Library.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Redmine.Library.Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly HttpClient _client;
        private readonly IUriHelper _uriHelper;
        private readonly ISerializerHelper _serializerHelper;

        public ActivityService(HttpClient client, IUriHelper uriHelper, ISerializerHelper serializerHelper)
        {
            _client = client;
            _uriHelper = uriHelper;
            _serializerHelper = serializerHelper;
        }

        public async Task<List<Activity>> GetActivityListResponseAsync(string authKey, int projectId)
        {
            if (string.IsNullOrEmpty(authKey))
                throw new ArgumentNullException(nameof(authKey));

            var toReturn = "";
            var requestUri = _uriHelper.Activities(projectId, authKey);
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                toReturn = await response.Content.ReadAsStringAsync();
                var parsedObject = JObject.Parse(toReturn);
                var timeEntriesArray = parsedObject["time_entry_activities"].ToString();
                var activityListResponse = JsonConvert.DeserializeObject<List<Activity>>(timeEntriesArray, _serializerHelper.SerializerSettings());
                return activityListResponse;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Invalid access key");
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new HttpRequestException("Not found");
                }
                var errormsg = await response.Content.ReadAsStringAsync();
                throw new Exception(errormsg);
            }
        }
    }
}