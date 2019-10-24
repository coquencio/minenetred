using Minenetred.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minenetred.Web.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetOpenProjectsAsync(string apiKey);
    }
}