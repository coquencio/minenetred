using Microsoft.Extensions.Logging;
using Minenetred.Web.Context;
using Minenetred.Web.Context.ContextModels;
using Minenetred.Web.Infrastructure;
using Redmine.Library.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.Web.Services.Implementations
{
    public class UsersManagementService : IUsersManagementService
    {
        private readonly MinenetredContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;
        private readonly IConnectionService _connectionService;
        private readonly ILogger<IUsersManagementService> _logger;

        public UsersManagementService(
            MinenetredContext context,
            IEncryptionService encryptionService,
            IUserService userService,
            ILogger<IUsersManagementService>logger,
            IConnectionService connectionService
            )
        {
            _context = context;
            _encryptionService = encryptionService;
            _userService = userService;
            _logger = logger;
            _connectionService = connectionService;
        }

        public bool IsUserRegistered(string userEmail)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == userEmail);
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
            _logger.LogInformation("Registered user:" + userEmail);
        }

        public bool HasRedmineKey(string userEmail)
        {
            if (string.IsNullOrEmpty(_context.Users.SingleOrDefault(u => u.UserName == userEmail).RedmineKey))
                return false;

            return true;
        }

        public void UpdateKey(string apiKey, string userEmail)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == userEmail);
            var encryptedKey = _encryptionService.Encrypt(apiKey);
            user.LastKeyUpdatedDate = DateTime.Now;
            user.RedmineKey = encryptedKey;
            _context.Users.Update(user);
            _context.SaveChanges();
            _logger.LogInformation("Updated redmine Key");
        }

        public void updateBaseAddress(string address, string email)
        {
            if (!Uri.IsWellFormedUriString(address, UriKind.Absolute))
            {
                throw new Exception("Invalid Uri");
            }
            var user = _context.Users.SingleOrDefault(u => u.UserName == email);
            user.BaseUri = address;
            _context.Users.Update(user);
            _context.SaveChanges();
            _connectionService.UpdateBaseAddress(address);
            _logger.LogInformation("Updated base address");
        }
        public async Task<bool> IsValidBaseAddressAsync()
        {
            var response = await _connectionService.CheckBaseAddressAsync();
            if (response == System.Net.HttpStatusCode.OK)
                return true;

            _logger.LogError("Invalid address");
            return false;
        }
        public void SetGlobalAddress(string email)
        {
            _connectionService.UpdateBaseAddress(_context.Users.SingleOrDefault(c=>c.UserName==email).BaseUri);
        }

        public string GetUserKey(string userEmail)
        {
            var EncryptedKey = _context.Users.SingleOrDefault(u => u.UserName == userEmail).RedmineKey;
            if (EncryptedKey == null)
                return null;

            return _encryptionService.Decrypt(EncryptedKey);
        }

        public async Task AddRedmineIdAsync(string key, string email)
        {
            var redmineUser = await _userService.GetCurrentUserAsync(key);
            var contextUser = _context.Users.SingleOrDefault(u => u.UserName == email);
            contextUser.RedmineId = redmineUser.Id;
            _context.Users.Update(contextUser);
            _context.SaveChanges();
        }
        public int GetRedmineId(string redmineKey = null, string userName = null)
        {
            if (userName != null)
            {
                return _context.Users.SingleOrDefault(u => u.UserName == userName).RedmineId;
            }
            if (redmineKey != null)
            {
                var encryptedKey = _encryptionService.Encrypt(redmineKey);
                return _context.Users.SingleOrDefault(u => u.RedmineKey== encryptedKey).RedmineId;
            }
            return 0;
        }
        public bool HasRedmineAddress(string emailAddress)
        {
            if (string.IsNullOrEmpty(_context.Users.SingleOrDefault(c=>c.UserName == emailAddress).BaseUri))
                return false;

            return true;
        }

        public async Task<string> GetBaseAddresAsync(string email)
        {
            if (HasRedmineAddress(email) && await IsValidBaseAddressAsync())
                return _context.Users.SingleOrDefault(c => c.UserName == email).BaseUri;

            return null;
        }
        public async Task<bool> IsApiKeyValidAsync(string apiKey)
        {
            return await _connectionService.IsApiKeyValid(apiKey);
        }
    }
}