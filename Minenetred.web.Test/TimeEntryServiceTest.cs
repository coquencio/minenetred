using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minenetred.web.Context;
using Minenetred.web.Context.ContextModels;
using Minenetred.web.Infrastructure;
using Minenetred.web.Services;
using Minenetred.web.Services.Implementations;
using Moq;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Minenetred.web.Test
{
    public class TimeEntryServiceTest
    {
        private TimeEntryService _timeEntryService;
        private Mock<Redmine.library.Services.ITimeEntryService> _timeEntryLibraryService;
        private IMapper _mapper;
        private MinenetredContext _context;
        private Mock<IUsersManagementService> _userManagementService;
        public TimeEntryServiceTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var options = new DbContextOptionsBuilder<MinenetredContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBaseTimeEntryService").Options;

            _timeEntryLibraryService = new Mock<Redmine.library.Services.ITimeEntryService>();
            _userManagementService = new Mock<IUsersManagementService>();
            _context = new MinenetredContext(options);
            _mapper = mappingConfig.CreateMapper();

            _timeEntryService = new TimeEntryService(
                _context,
                _timeEntryLibraryService.Object,
                _mapper,
                _userManagementService.Object
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

            var timeEntryResponse = new TimeEntryListResponse()
            {
                Time_Entries = timeEntryListResponse,
            };
            async Task<TimeEntryListResponse> AssignResponse()
            {
                await Task.Delay(0);
                return timeEntryResponse;
            }

            //_context.Users.Add();
            _userManagementService.Setup(s=> s.GetUserKey(testUserName)).Returns(testKey);
            _timeEntryLibraryService
                .Setup(s=>s.GetTimeEntriesAsync(testKey, redmineIdForTest, projectIdTest, testDate))
                .Returns(AssignResponse());

            Assert.Equal(total, await _timeEntryService.GetTimeEntryHoursPerDay(projectIdTest, testDate, testUserName));

        }
    }
}
