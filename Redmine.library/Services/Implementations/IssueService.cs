﻿using Newtonsoft.Json;
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
    public class IssueService : IIssueService
    {
        private readonly HttpClient _client;
        private readonly IUriHelper _uriHelper;

        public IssueService(HttpClient client, IUriHelper uriHelper)
        {
            _client = client;
            _uriHelper = uriHelper;
            if (ClientSettings.BaseAddress != null)
            {
                _client.BaseAddress = new Uri(ClientSettings.BaseAddress);
            }
        }

        public async Task<List<Issue>> GetIssuesAsync(string authKey, int assignedToId, int projectId)
        {
            if (string.IsNullOrEmpty(authKey))
                throw new ArgumentNullException(nameof(authKey));

            var toReturn = "";
            var requestUri = _uriHelper.Issues(authKey, assignedToId, projectId);
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                toReturn = await response.Content.ReadAsStringAsync();
                var parsedObject = JObject.Parse(toReturn);
                var issuesArray = parsedObject["issues"].ToString();
                var issueResponse = JsonConvert.DeserializeObject<List<Issue>>(issuesArray);
                return issueResponse;
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