using AutoMapper;
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
    public class ActivityServiceTest
    {
        private ActivityService _activityService;
        private IMapper _mapper;
        private Mock<Redmine.library.Services.IActivityService> _activityLibraryService;
        private Mock<IUsersManagementService> _usersManagementService;

        public ActivityServiceTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mappingConfig.CreateMapper();
            _activityLibraryService = new Mock<Redmine.library.Services.IActivityService>();
            _usersManagementService = new Mock<IUsersManagementService>();
            _activityService = new ActivityService(
                _mapper,
                _usersManagementService.Object,
                _activityLibraryService.Object
                );

        }

        [Fact]
        public async Task ShouldMapActivityPropertiesToViewModelAsync()
        {
            var userNameTest = "testUser";
            var decryptedKey = "testKey";
            var projectIdTest = 0;
            var activityForTest1 = new Activity()
            {
                Id = 1,
                Name="Meeting"
            };
            var activityForTest2 = new Activity()
            {
                Id = 2,
                Name = "Testing"
            };
            var activitiesList = new List<Activity>();
            activitiesList.Add(activityForTest1);
            activitiesList.Add(activityForTest2);

            var responseList = new ActivityListResponse()
            {
                TimeEntryActivities = activitiesList,
            };
            async Task<ActivityListResponse> AssignResponse()
            {
                await Task.Delay(0);
                return responseList;
            }
            _usersManagementService.Setup(c=>c.GetUserKey(userNameTest)).Returns(decryptedKey);
            _activityLibraryService.Setup(s => s.GetActivityListResponseAsync(decryptedKey, projectIdTest)).Returns(AssignResponse());

            var activitiesViewModel = await _activityService.GetActivitiesAsync(projectIdTest, userNameTest);
            int counter = 1;
            foreach (var activity in activitiesViewModel.Activities)
            {
                if (counter==1)
                {
                    Assert.Equal(activity.Id, activityForTest1.Id);
                    Assert.Equal(activity.Name, activityForTest1.Name);
                }
                else
                {
                    Assert.Equal(activity.Id, activityForTest2.Id);
                    Assert.Equal(activity.Name, activityForTest2.Name);
                }
                counter += 1;
            }

        }
    }
}
