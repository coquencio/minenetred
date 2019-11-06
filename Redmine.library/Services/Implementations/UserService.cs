using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redmine.Library.Core;
using Redmine.Library.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Redmine.Library.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserServiceModel> GetCurrentUserAsync(string authKey)
        {
            if (string.IsNullOrEmpty(authKey))
                throw new ArgumentNullException(nameof(authKey));

            var toReturn = "";
            var requestUri = Constants.CurrentUser + Constants.Json + "?key=" + authKey;
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                toReturn = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(toReturn);
                var userData = jsonObject["user"].ToString();
                var user = JsonConvert.DeserializeObject<UserServiceModel>(userData);
                return user;
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