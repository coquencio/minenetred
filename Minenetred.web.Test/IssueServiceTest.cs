using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minenetred.Web.Context;
using Minenetred.Web.Context.ContextModels;
using Minenetred.Web.Infrastructure;
using Minenetred.Web.Services;
using Minenetred.Web.Services.Implementations;
using Moq;
using Redmine.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Minenetred.Web.Test
{
    public class IssueServiceTest
    {
        private IssueService _issueService;
        private MinenetredContext _context;
        private Mock<Redmine.Library.Services.IIssueService> _issueLibraryService;
        private IMapper _mapper;
        private Mock<IUsersManagementService> _usersManagementService;

        public IssueServiceTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var options = new DbContextOptionsBuilder<MinenetredContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBaseIssueService").Options;

            _context = new MinenetredContext(options);
            _mapper = mappingConfig.CreateMapper();
            _issueLibraryService = new Mock<Redmine.Library.Services.IIssueService>();
            _usersManagementService = new Mock<IUsersManagementService>();
            _issueService = new IssueService(
                _context,
                _issueLibraryService.Object,
                _mapper,
                _usersManagementService.Object
                );
        }

        [Fact]
        public async Task ShouldMapAllIssuePropertiesAsync()
        {
            var userNameForTest = "test user";
            var keyForTest = "test key";
            var redmineIdForTest = 0;
            var projectIdForTest = 1;

            var userForTest = new User()
            {
                UserName = userNameForTest,
                CreatedDate = DateTime.Now,
                RedmineId = redmineIdForTest
            };
            _context.Users.Add(userForTest);
            _context.SaveChanges();
            _usersManagementService.Setup(s => s.GetUserKey(userNameForTest)).Returns(keyForTest);

            var issueForTest1 = new Issue()
            {
                Id = 1,
                Subject = "subject test 1",
                Description = "description test 1",
            };
            var issueForTest2 = new Issue()
            {
                Id = 2,
                Subject = "subject test 2",
                Description = "description test 2",
            };

            var listToAdd = new List<Issue>();
            listToAdd.Add(issueForTest1);
            listToAdd.Add(issueForTest2);

            async Task<List<Issue>> AssignResponse()
            {
                await Task.Delay(0);
                return listToAdd;
            }
            _issueLibraryService.Setup(s => s.GetIssuesAsync(keyForTest, redmineIdForTest, projectIdForTest)).Returns(AssignResponse());
            var issueViewModel = await _issueService.GetIssuesAsync(projectIdForTest, userNameForTest);
            var counter = 1;
            foreach (var issue in issueViewModel)
            {
                if (counter == 1)
                {
                    Assert.Equal(issueForTest1.Id, issue.Id);
                    Assert.Equal(issueForTest1.Subject, issue.Subject);
                    Assert.Equal(issueForTest1.Description, issue.Description);
                }
                else
                {
                    Assert.Equal(issueForTest2.Id, issue.Id);
                    Assert.Equal(issueForTest2.Subject, issue.Subject);
                    Assert.Equal(issueForTest2.Description, issue.Description);
                }
                counter += 1;
            }
        }
    }
}