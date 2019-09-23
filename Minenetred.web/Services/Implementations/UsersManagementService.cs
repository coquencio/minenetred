using Minenetred.web.Context;
using Minenetred.web.Context.ContextModels;
using Minenetred.web.Infrastructure;
using Redmine.library.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Services.Implementations
{
    public class UsersManagementService : IUsersManagementService
    {
        private readonly MinenetredContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;
        public UsersManagementService(
            MinenetredContext context,
            IEncryptionService encryptionService,
            IUserService userService
            )
        {
            _context = context;
            _encryptionService = encryptionService;
            _userService = userService;
        }
        
        public bool CheckReisteredUser(string userEmail)
        {
            var user = _context.Users.SingleOrDefault(u=>u.UserName==userEmail);
            if (user == null)
                return false;

            return true;
        }

        public void RegisterUser(string userEmail)
        {
            var newUser = new User()
            {
                UserName = userEmail,
                CreatedDate = DateTime.Now,
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public bool CheckRedmineKey(string userEmail)
        {
            if (_context.Users.SingleOrDefault(u => u.UserName == userEmail).RedmineKey == null)
                return false;

            return true;
        }

        public void UpdateKey(string apiKey, string userEmail)
        {
            var user = _context.Users.SingleOrDefault(u=> u.UserName == userEmail);
            var encryptedKey = _encryptionService.Encrypt(apiKey);
            user.LastKeyUpdatedDate = DateTime.Now;
            user.RedmineKey = encryptedKey;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public string GetUserKey(string userEmail)
        {
            var EncryptedKey = _context.Users.SingleOrDefault(u=>u.UserName == userEmail).RedmineKey;
            if (EncryptedKey == null)
                return null;

            return _encryptionService.Decrypt(EncryptedKey);
        }

        public async Task AddRedmineIdAsync(string key)
        {
            var redmineUser = await _userService.GetCurrentUserAsync(key);
            var contextUser = _context.Users.SingleOrDefault(u=>u.UserName == UserPrincipal.Current.EmailAddress);
            contextUser.RedmineId = redmineUser.User.Id;
            _context.Users.Update(contextUser);
            _context.SaveChanges();
        }
    }
}
