using Redmine.Library.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.Library.Services.Implementations
{
    public class ConnectionService : IConnectionService
    {
        private readonly HttpClient _client;
        public ConnectionService(HttpClient client)
        {
            _client = client;
            if (ClientSettings.BaseAddress != null)
            {
                _client.BaseAddress = new Uri(ClientSettings.BaseAddress);
            }
        }

        public void UpdateBaseAddress(string Address)
        {
            ClientSettings.BaseAddress = Address;
            _client.BaseAddress = new Uri(ClientSettings.BaseAddress);
        }

        public async Task<HttpStatusCode> CheckBaseAddressAsync()
        {
            HttpResponseMessage response =  await _client.GetAsync("");
            return response.StatusCode;
        }
        public async Task<bool> CheckApiKeyAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return false;
            }
            string address = Constants.CurrentUser + Constants.Json + "&" + Constants.Key + apiKey;
            HttpResponseMessage response = await _client.GetAsync(address);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }
    }
}
