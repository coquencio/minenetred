using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface IUsersManagementService
    {
        bool IsUserRegistered(string userEmail);

        void RegisterUser(string userEmail);

        bool HasRedmineKey(string userEmail);

        void UpdateKey(string apiKey, string userEmail);

        string GetUserKey(string userEmail);

        Task AddRedmineIdAsync(string key, string email);

        int GetRedmineId(string redmineKey = null, string userName = null);
    }
}