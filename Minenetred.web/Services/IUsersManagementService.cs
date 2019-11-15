using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface IUsersManagementService
    {
        bool IsUserRegistered(string userEmail);

        void RegisterUser(string userEmail);

        bool HasRedmineKey(string userEmail);

        void UpdateKey(string apiKey, string userEmail);
        void updateBaseAddress(string address, string email);
        Task<bool> IsValidBaseAddressAsync();
        void SetGlobalAddress(string email);

        string GetUserKey(string userEmail);

        Task AddRedmineIdAsync(string key, string email);

        int GetRedmineId(string redmineKey = null, string userName = null);
        bool HasRedmineAddress(string emailAddress);
        Task<string> GetBaseAddresAsync(string email);
    }
}