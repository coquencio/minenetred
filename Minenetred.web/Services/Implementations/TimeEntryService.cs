using AutoMapper;
using Minenetred.web.Context;
using Minenetred.web.Infrastructure;
using Minenetred.web.Models;
using Minenetred.web.ViewModels;
using Newtonsoft.Json;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Minenetred.web.Services.Implementations
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly MinenetredContext _context;
        private readonly Redmine.library.Services.ITimeEntryService _timeEntryService;
        private readonly IMapper _mapper;
        private readonly IUsersManagementService _usersManagementService;

        public TimeEntryService(
            MinenetredContext context,
            Redmine.library.Services.ITimeEntryService timeEntryService,
            IMapper mapper,
            IUsersManagementService usersManagementService
            )
        {
            _context = context;
            _timeEntryService = timeEntryService;
            _mapper = mapper;
            _usersManagementService = usersManagementService;
        }

        public async Task<float> GetTimeEntryHoursPerDay(int projectId, string date, string user)
        {
           var key = _usersManagementService.GetUserKey(user);
           var redmineId = _context.Users.SingleOrDefault(u=>u.UserName == user).RedmineId;
           var response = await _timeEntryService.GetTimeEntriesAsync(key, redmineId, projectId, date);
           var shapedList = _mapper.Map<TimeEntryListResponse, TimeEntryViewModel>(response);
            float totalHours = 0;
            foreach (var entry in shapedList.TimeEntries)
            {
                totalHours += entry.Hours;
            }
           return totalHours;
        }

        public async Task<HttpStatusCode> AddTimeEntryAsync(TimeEntryFormDto entry)
        {
            var entryToMap = new TimeEntryFormContainer()
            {
                TimeEntry = entry,
            };
            var timeEntry = _mapper.Map<TimeEntryFormContainer, TimeEntryDtoContainer>(entryToMap);
            var key = _usersManagementService.GetUserKey(UserPrincipal.Current.EmailAddress);
            return  await _timeEntryService.AddTimeEntryAsync(timeEntry, key);
        }
    }
}
