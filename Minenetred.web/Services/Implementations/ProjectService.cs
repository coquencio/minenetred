using AutoMapper;
using Minenetred.Web.Models;
using Redmine.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minenetred.Web.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly Redmine.Library.Services.IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectService(
            Redmine.Library.Services.IProjectService projectService,
            IMapper mapper
            )
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetOpenProjectsAsync(string apiKey)
        {
            var response = await _projectService.GetProjectsAsync(apiKey);
            var projectList = _mapper.Map<List<Project>, List<ProjectDto>>(response);
            var shapedList = new List<ProjectDto>();

            foreach (var project in projectList)
            {
                if (project.Status == 1)
                {
                    shapedList.Add(project);
                }
            }
            return shapedList;
        }
    }
}