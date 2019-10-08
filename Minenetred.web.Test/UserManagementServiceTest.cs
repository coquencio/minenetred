using Microsoft.EntityFrameworkCore;
using Minenetred.web.Context;
using Minenetred.web.Infrastructure;
using Minenetred.web.Services;
using Minenetred.web.Services.Implementations;
using Moq;
using Redmine.library.Models;
using Redmine.library.Services;
using Redmine.library.Services.Implementations;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Minenetred.web.Test
{
    public class UserManagementServiceTest
    {
        private MinenetredContext _context;
        private EncryptionService _encryptionService;
        private UsersManagementService _usersManagementService;
        private Mock<IUserService> _userService;
        
        public UserManagementServiceTest()
        {
            var options = new DbContextOptionsBuilder<MinenetredContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBaseUserManagementService").Options;
            _context = new MinenetredContext(options);
            _encryptionService = new EncryptionService("DefaultTest");
            _userService = new Mock<IUserService>();
            _usersManagementService = new UsersManagementService(
                _context,
                _encryptionService,
                _userService.Object
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
            var returnedUser = new UserResponse()
            {
                User = new UserServiceModel()
                {
                    Id = 5
                },
            };
            async Task<UserResponse> AssignResponse()
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
