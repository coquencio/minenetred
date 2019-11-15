using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Minenetred.Web.Context;
using Minenetred.Web.Infrastructure;
using Minenetred.Web.Services.Implementations;
using Moq;
using Redmine.Library.Models;
using Redmine.Library.Services;
using System.Threading.Tasks;
using Xunit;

namespace Minenetred.Web.Test
{
    public class UserManagementServiceTest
    {
        private MinenetredContext _context;
        private EncryptionService _encryptionService;
        private UsersManagementService _usersManagementService;
        private Mock<IUserService> _userService;
        private Mock<ILogger<UsersManagementService>> _logger;
        private Mock<IConnectionService> _connectionService;

        public UserManagementServiceTest()
        {
            var options = new DbContextOptionsBuilder<MinenetredContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBaseUserManagementService").Options;
            _context = new MinenetredContext(options);
            _encryptionService = new EncryptionService("DefaultTest");
            _userService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UsersManagementService>>();
            _connectionService = new Mock<IConnectionService>();
            _usersManagementService = new UsersManagementService(
                _context,
                _encryptionService,
                _userService.Object,
                _logger.Object,
                _connectionService.Object
                );
        }

        [Fact]
        public void ShouldCheckIfUserExistAfterCreation()
        {
            var userName = "TestUser1";
            _usersManagementService.RegisterUser(userName);
            var check = _usersManagementService.IsUserRegistered(userName);
            Assert.True(check);
        }

        [Fact]
        public void ShouldChekIfRedMineKeyExistAfterAddingIt()
        {
            var userName = "TestUser2";
            var redMineKey = "TestKey";

            _usersManagementService.RegisterUser(userName);
            _usersManagementService.UpdateKey(redMineKey, userName);
            Assert.True(_usersManagementService.HasRedmineKey(userName));
        }

        [Fact]
        public async Task ShouldRegisterRedmineIdFromLibraryServiceAsync()
        {
            var testKey = "TestKey";
            var testUser = "TestUser3";
            var returnedUser = new UserServiceModel()
            {
                Id = 5
            };
            async Task<UserServiceModel> AssignResponse()
            {
                await Task.Delay(0);
                return returnedUser;
            }
            _userService.Setup(s => s.GetCurrentUserAsync(testKey)).Returns(AssignResponse());
            _usersManagementService.RegisterUser(testUser);
            await _usersManagementService.AddRedmineIdAsync(testKey, testUser);
            var userToCheck = await _context.Users.SingleOrDefaultAsync(c => c.RedmineId != 0);
            Assert.Equal(5, userToCheck.RedmineId);
        }
    }
}