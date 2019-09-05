using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redmine.library.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Redmine.library
{
    public class Projects 
    {
        private HttpClient _client { get; set; }
        //public string uspas { get; set; }

        public Projects()
        {
            //uspas = "user:pass";
            //System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(uspas))
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.baseAddress);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Constants.basicAuth,Constants.testToken);

        }


        public async Task<ProjectsContent> GetProjects()
        {
            string toReturn = "";
            string requestUri = Constants.projects + Constants.json;
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
                toReturn = await response.Content.ReadAsStringAsync();

            ProjectsContent projectsContent =  JsonConvert.DeserializeObject<ProjectsContent>(toReturn); 
            return projectsContent;

        }

    }
}
