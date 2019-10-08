using AutoMapper;
using Minenetred.web.Context;
using Minenetred.web.ViewModels;
using Redmine.library.Models;
using Redmine.library.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly IMapper _mapper;
        private readonly IUsersManagementService _usersManagementService;
        private readonly Redmine.library.Services.IActivityService _activityService;

        public ActivityService(
            IMapper mapper,
            IUsersManagementService usersManagementService,
            Redmine.library.Services.IActivityService activityService
            )
        {
            _mapper = mapper;
            _usersManagementService = usersManagementService;
            _activityService = activityService;
        }

        public async Task<ActivityViewModel> GetActivitiesAsync(int projectId, string email)
        {
            var userName = email;
            var decryptedKey = _usersManagementService.GetUserKey(userName);
            var retrievedData = await _activityService.GetActivityListResponseAsync(decryptedKey, projectId);
            var toRetun = _mapper.Map<ActivityListResponse, ActivityViewModel>(retrievedData);
            return toRetun;
        }
    }
}
