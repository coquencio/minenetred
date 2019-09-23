using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetCurrentUserAsync(string authKey);
    }
}
