using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.Library.Services
{
    public interface IConnectionService
    {
        void UpdateBaseAddress(string Address);
        Task<HttpStatusCode> CheckBaseAddressAsync();
        Task<bool> IsApiKeyValid(string apiKey);
    }
}
