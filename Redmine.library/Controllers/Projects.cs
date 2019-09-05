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
    // todo: rename to Project Service and IProjectService interface
    public class Projects 
    {
        private HttpClient _client { get; set; }
        //public string uspas { get; set; }

        public Projects()
        {
            //uspas = "user:pass";
            //System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(uspas))
            // todo: We should not instantiate HttpClient each time. There are issues with this approach.
            // It should be passed from the consumer (web app). 
            //Please refer to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.baseAddress);
            _client.DefaultRequestHeaders.Clear();
            // Authorization should be provided by the consumer app.
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Constants.basicAuth,Constants.testToken);

        }


        public async Task<ProjectsContent> GetProjects()
        {
            // todo: wrap this in a try catch method. We'll handle exceptions later.
            // todo: use var instead of explicit type declaration when possible
            string toReturn = "";
            string requestUri = Constants.projects + Constants.json;
            HttpResponseMessage response = await _client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
                toReturn = await response.Content.ReadAsStringAsync();

            // todo: this will throw an exception if the response failed. Overall the method could look more like
            // if(response.IsSuccessStatusCode) {
            // var content = await response.Content.ReadAsStringAsync();
            // return JsonConver.DeserializeObject<ProjectsContent>(content)
            //}
            // else {
            // throw new Exception(response.Error or something)
            //}
            ProjectsContent projectsContent =  JsonConvert.DeserializeObject<ProjectsContent>(toReturn); 
            return projectsContent;

        }

    }
}
