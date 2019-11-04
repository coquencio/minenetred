using AutoMapper;
using Minenetred.Web.Context;
using Minenetred.Web.Models;
using Newtonsoft.Json.Linq;
using Redmine.Library.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Minenetred.Web.Services.Implementations
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly MinenetredContext _context;
        private readonly Redmine.Library.Services.ITimeEntryService _timeEntryService;
        private readonly IMapper _mapper;
        private readonly IUsersManagementService _usersManagementService;
        private readonly IProjectService _projectService;

        public TimeEntryService(
            MinenetredContext context,
            Redmine.Library.Services.ITimeEntryService timeEntryService,
            IMapper mapper,
            IUsersManagementService usersManagementService,
            IProjectService projectService
            )
        {
            _context = context;
            _timeEntryService = timeEntryService;
            _mapper = mapper;
            _usersManagementService = usersManagementService;
            _projectService = projectService;
        }

        public async Task<float> GetTimeEntryHoursPerDay(int projectId, string date, string user)
        {
            var key = _usersManagementService.GetUserKey(user);
            var redmineId = _context.Users.SingleOrDefault(u => u.UserName == user).RedmineId;
            var response = await _timeEntryService.GetTimeEntriesAsync(key, redmineId, projectId, date, date);
            var shapedList = _mapper.Map<List<TimeEntry>, List<Models.TimeEntryDto>>(response);

            float totalHours = 0;
            foreach (var entry in shapedList)
            {
                totalHours += entry.Hours;
            }
            return totalHours;
        }

        public async Task<HttpStatusCode> AddTimeEntryAsync(JObject entry)
        {
            var jsonObject = entry;
            int issueId = Convert.ToInt32(jsonObject["issueId"]);
            string spentOn = jsonObject["spentOn"].ToString();
            double hours = Convert.ToDouble(jsonObject["hours"]);
            int activityId = Convert.ToInt32(jsonObject["activityId"]);
            string comments = jsonObject["comments"].ToString();
            var key = _usersManagementService.GetUserKey(UserPrincipal.Current.EmailAddress);
            return await _timeEntryService.AddTimeEntryAsync(key, issueId, spentOn, hours, activityId, comments);
        }

        private async Task<List<DateTime>> GetFutureTimeEntriesDates(DateTime today, string apiKey, DateTime lastPeriodDate)
        {
            var toReturn = new List<DateTime>();
            today = today.AddDays(1);
            var projects = await _projectService.GetOpenProjectsAsync(apiKey);
            var redimeId = _usersManagementService.GetRedmineId(apiKey);
            var fromDate = today.ToString("yyyy-MM-dd");
            var toDate = lastPeriodDate.ToString("yyyy-MM-dd");
            foreach (var project in projects)
            {
                var time = await _timeEntryService.GetTimeEntriesAsync(apiKey, redimeId, project.Id, fromDate, toDate);
                if (time.Any())
                {
                    foreach (var entry in time)
                    {
                        toReturn.Add(entry.SpentOn);
                    }
                }
            }
            return toReturn;
        }

        private DateTime GetFirstPeriodDay(DateTime today)
        {
            if (today.Day <= 15)
            {
                return new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                return new DateTime(today.Year, today.Month, 16);
            }
        }

        public DateTime GetLastPeriodDay(DateTime today)
        {
            if (today.Day <= 15)
            {
                return new DateTime(today.Year, today.Month, 15);
            }
            else
            {
                return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            }
        }

        public async Task<Dictionary<String, int>> GetUnloggedDaysAsync(int UserId, string authKey, DateTime today)
        {
            var toReturn = new Dictionary<String, int>();
            var firstPeriodDay = GetFirstPeriodDay(today);
            var lastPeriodDay = GetLastPeriodDay(today);
            var futureTimeEntries = await GetFutureTimeEntriesDates(today, authKey, lastPeriodDay);
            var referenceDate = new DateTime();
            if (futureTimeEntries.Any())
            {
                referenceDate = futureTimeEntries.Max<DateTime>();
            }
            else
            {
                referenceDate = today;
            }
            double numberOfDays = (referenceDate - firstPeriodDay).TotalDays;
            for (int i = 0; i < numberOfDays; i++)
            {
                var dateToValidate = firstPeriodDay.AddDays(i);
                float hoursPerDay = 0;
                var entries = await _timeEntryService.GetTimeEntriesAsync(
                    authKey,
                    UserId,
                    fromDate: dateToValidate.ToString("yyyy-MM-dd"),
                    toDate: dateToValidate.ToString("yyyy-MM-dd"));
                foreach (var entry in entries)
                {
                    if (entry.Activity.Name.Equals("Vacation/PTO/Holiday"))
                    {
                        hoursPerDay = 8;
                        continue;
                    }
                    hoursPerDay += entry.Hours;
                }
                if ((dateToValidate.DayOfWeek.ToString().Equals("Saturday") || dateToValidate.DayOfWeek.ToString().Equals("Sunday")))
                {
                    if (hoursPerDay != 0)
                        toReturn.Add(dateToValidate.ToString("yyyy-MM-dd"), 1);
                }
                else
                {
                    if (hoursPerDay < 8)
                        toReturn.Add(dateToValidate.ToString("yyyy-MM-dd"), 0);
                }
            }
            return toReturn;
        }
    }
}