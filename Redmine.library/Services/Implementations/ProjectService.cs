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
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _client;
        private readonly IUriHelper _uriHelper;
        private readonly ISerializerHelper _serializerHelper;

        public ProjectService(HttpClient client, IUriHelper uriHelper, ISerializerHelper serializerHelper)
        {
            _client = client;
            _uriHelper = uriHelper;
            _serializerHelper = serializerHelper;
        }

        public async Task<List<Project>> GetProjectsAsync(string authKey)
        {
            if (authKey == null || authKey.Equals(""))
                throw new ArgumentNullException(nameof(authKey));

            var toReturn = "";
            var requestUri = _uriHelper.Projects(authKey);
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                toReturn = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(toReturn);
                var projects = jsonObject["projects"].ToString();
                var projectListResponse = JsonConvert.DeserializeObject<List<Project>>(projects, _serializerHelper.SerializerSettings());
                return projectListResponse;
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