using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redmine.library.Models;
using Redmine.library.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _client;

        public ProjectService(HttpClient client)
        {
            _client = client;
        }


        public async Task<ProjectListResponse> GetProjectsAsync(string authKey)
        {
  
            try
            {
                if (authKey == null || authKey.Equals(""))
                    throw new ArgumentNullException("Key hasn't been implemented yet");

                var toReturn = "";
                var requestUri = Constants.projects + Constants.json+ "?key=" + authKey;
                HttpResponseMessage response = await _client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    toReturn = await response.Content.ReadAsStringAsync();
                    var projectListResponse = JsonConvert.DeserializeObject<ProjectListResponse>(toReturn);
                    return projectListResponse;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
            

           


        }

    }
}
