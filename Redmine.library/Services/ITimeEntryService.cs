using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redmine.library.Services
{
    public interface ITimeEntryService
    {
        Task<TimeEntryListResponse> GetTimeEntriesAsync(string authKey, int userId, int projectId, string date);
    }
}
