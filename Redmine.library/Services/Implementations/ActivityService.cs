﻿using Newtonsoft.Json;
using Redmine.library.Core;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly HttpClient _client;

        public ActivityService(HttpClient client)
        {
            _client = client;
        }
        public async Task<ActivityListResponse> GetActivityListResponseAsync(string authKey, int projectId)
        {
            try
            {
                if (string.IsNullOrEmpty(authKey))
                    throw new ArgumentNullException(Constants.nullKeyException);

                var toReturn = "";
                var requestUri = 
                    Constants.Activites +
                    Constants.json +
                    "?key=" +
                    authKey +
                    "&" +
                    Constants.projectId +
                    projectId;
                HttpResponseMessage response = await _client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    toReturn = await response.Content.ReadAsStringAsync();
                    var activityListResponse = JsonConvert.DeserializeObject<ActivityListResponse>(toReturn);
                    return activityListResponse;
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMsg);
                }   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}