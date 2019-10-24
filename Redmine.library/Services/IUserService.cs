using Redmine.Library.Models;
using System.Threading.Tasks;

namespace Redmine.Library.Services
{
    public interface IUserService
    {
        Task<UserServiceModel> GetCurrentUserAsync(string authKey);
    }
}