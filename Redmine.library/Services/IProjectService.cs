using Redmine.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redmine.Library.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsAsync(string authKey);
    }
}