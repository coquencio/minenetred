using AutoMapper;
using Minenetred.web.Context;
using Minenetred.web.Infrastructure;
using Minenetred.web.ViewModels;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Services.Implementations
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly MinenetredContext _context;
        private readonly Redmine.library.Services.ITimeEntryService _timeEntryService;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly IUsersManagementService _usersManagementService;

        public TimeEntryService(
            MinenetredContext context,
            Redmine.library.Services.ITimeEntryService timeEntryService,
            IEncryptionService encryptionService,
            IMapper mapper,
            IUsersManagementService usersManagementService
            )
        {
            _context = context;
            _timeEntryService = timeEntryService;
            _encryptionService = encryptionService;
            _mapper = mapper;
            _usersManagementService = usersManagementService;
        }

        public async Task<TimeEntryViewModel> GetTimeEntriesAsync(int projectId, string date)
        {
           var user = UserPrincipal.Current.EmailAddress;
           var key = _usersManagementService.GetUserKey(user);
           var redmineId = _context.Users.SingleOrDefault(u=>u.UserName == user).RedmineId;
           var response = await _timeEntryService.GetTimeEntriesAsync(key, redmineId, projectId, date);
           var toReturn = _mapper.Map<TimeEntryListResponse, TimeEntryViewModel>(response);
           return toReturn;
        }
    }
}
