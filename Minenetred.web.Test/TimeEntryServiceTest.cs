using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minenetred.Web.Context;
using Minenetred.Web.Context.ContextModels;
using Minenetred.Web.Infrastructure;
using Minenetred.Web.Models;
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
    public class TimeEntryServiceTest
    {
        private TimeEntryService _timeEntryService;
        private Mock<Redmine.Library.Services.ITimeEntryService> _timeEntryLibraryService;
        private IMapper _mapper;
        private MinenetredContext _context;
        private Mock<IUsersManagementService> _userManagementService;
        private Mock<IProjectService> _projectService;

        public TimeEntryServiceTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var options = new DbContextOptionsBuilder<MinenetredContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBaseTimeEntryService").Options;

            _timeEntryLibraryService = new Mock<Redmine.Library.Services.ITimeEntryService>();
            _userManagementService = new Mock<IUsersManagementService>();
            _context = new MinenetredContext(options);
            _mapper = mappingConfig.CreateMapper();
            _projectService = new Mock<IProjectService>();
            _timeEntryService = new TimeEntryService(
                _context,
                _timeEntryLibraryService.Object,
                _mapper,
                _userManagementService.Object,
                _projectService.Object
                );
        }

        [Theory]
        [InlineData(1, 3, 1, 5, "Test1")]
        [InlineData(2, 3.5, 0.5, 6, "Test2")]
        [InlineData(0.5, 1, 2, 3.5, "Test3")]
        public async Task ShouldCountHoursRetrievedAsync(
            float hours1,
            float hours2,
            float hours3,
            float total,
            string testUserName)
        {
            var projectIdTest = 0;
            var testDate = "TestDate";
            var testKey = "Test Key";
            var redmineIdForTest = 1;

            var userForTest = new User()
            {
                UserName = testUserName,
                RedmineId = redmineIdForTest,
                CreatedDate = DateTime.Now,
            };
            _context.Users.Add(userForTest);
            _context.SaveChanges();

            var timeEntry1 = new Mock<TimeEntry>();
            var timeEntry2 = new Mock<TimeEntry>();
            var timeEntry3 = new Mock<TimeEntry>();

            timeEntry1.Object.Hours = hours1;
            timeEntry2.Object.Hours = hours2;
            timeEntry3.Object.Hours = hours3;

            var timeEntryListResponse = new List<TimeEntry>();
            timeEntryListResponse.Add(timeEntry1.Object);
            timeEntryListResponse.Add(timeEntry2.Object);
            timeEntryListResponse.Add(timeEntry3.Object);

            async Task<List<TimeEntry>> AssignResponse()
            {
                await Task.Delay(0);
                return timeEntryListResponse;
            }

            _userManagementService.Setup(s => s.GetUserKey(testUserName)).Returns(testKey);
            _timeEntryLibraryService
                .Setup(s => s.GetTimeEntriesAsync(testKey, redmineIdForTest, projectIdTest, testDate, testDate))
                .Returns(AssignResponse());

            Assert.Equal(total, await _timeEntryService.GetTimeEntryHoursPerDay(projectIdTest, testDate, testUserName));
        }

        [Theory]
        [InlineData("Vacation/PTO/Holiday", 0)]
        [InlineData("Random activity", 8)]
        [InlineData("Random activity", 0)]
        public async Task ShouldReturnDictionaryOfFutureAsync(string activityName, int hours)
        {
            DateTime DateToTest = new DateTime(2019, 10, 11);
            int userTestId = 0;
            string authKeyTest = "testKey";
            int redmineIdTest = 0;

            Mock<ProjectDto> mockedProject = new Mock<ProjectDto>();
            var projectList = new List<ProjectDto>();
            projectList.Add(mockedProject.Object);

            async Task<List<ProjectDto>> AssignResponse()
            {
                await Task.Delay(0);
                return projectList;
            }
            async Task<List<TimeEntry>> timeEntryResponse()
            {
                await Task.Delay(0);
                return new List<TimeEntry>(); ;
            }

            Mock<TimeEntry> timeEntry1 = new Mock<TimeEntry>();
            timeEntry1.Object.SpentOn = new DateTime(2019, 10, 13);
            timeEntry1.Object.Hours = hours;
            timeEntry1.Object.Activity = new Activity()
            {
                Id = 1,
                Name = activityName,
            };

            var timeEntryListToShape = new List<TimeEntry>()
            {
                timeEntry1.Object,
            };

            async Task<List<TimeEntry>> timeEntryResponseToShape()
            {
                await Task.Delay(0);
                return timeEntryListToShape;
            }

            _projectService.Setup(s => s.GetOpenProjectsAsync(authKeyTest)).Returns(AssignResponse());
            _userManagementService.Setup(s => s.GetRedmineId(authKeyTest, null)).Returns(redmineIdTest);

            _timeEntryLibraryService.Setup(s =>
            s.GetTimeEntriesAsync(
                authKeyTest,
                redmineIdTest,
                0,
                "2019-10-12",
                "2019-10-15"))
                .Returns(timeEntryResponse());

            var dateTest = new DateTime(2019, 10, 01);
            for (int i = 0; i < 15; i++)
            {
                var dateForSetup = dateTest.AddDays(i).ToString("yyyy-MM-dd");
                _timeEntryLibraryService.Setup(s =>
                    s.GetTimeEntriesAsync(
                        authKeyTest,
                        redmineIdTest,
                        0,
                        dateForSetup,
                        dateForSetup))
                        .Returns(timeEntryResponseToShape());
            }
            var dictionaryToValidate = await _timeEntryService.GetUnloggedDaysAsync(userTestId, authKeyTest, DateToTest);

            if (activityName.Equals("Vacation/PTO/Holiday"))
            {
                //should validate all time entries where ignored except for the weekends
                Assert.True(dictionaryToValidate.Count == 2);
            }
            else
            {
                if (hours == 8)
                {
                    //should have 2 weekend logs warnings
                    Assert.True(dictionaryToValidate.Count == 2);
                    foreach (var entry in dictionaryToValidate)
                    {
                        Assert.Equal(1, entry.Value);
                    }
                }
                else
                {
                    //should check unlogged hours during the period
                    Assert.True(dictionaryToValidate.Count == 8);
                    foreach (var entry in dictionaryToValidate)
                    {
                        Assert.Equal(0, entry.Value);
                    }
                }
            }
        }

    }
}