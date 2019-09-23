using AutoMapper;
using Minenetred.web.Models;
using Minenetred.web.ViewModels;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly Redmine.library.Services.IProjectService _projectService;
        private readonly IMapper _mapper;
        public ProjectService(
            Redmine.library.Services.IProjectService projectService,
            IMapper mapper
            )
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        public async Task<ProjectsViewModel> GetOpenProjectsAsync(string apiKey)
        {
            var response = await _projectService.GetProjectsAsync(apiKey);
            var projectList = _mapper.Map<ProjectListResponse, ProjectsViewModel>(response);
            var shapedList = new ProjectsViewModel()
            {
                Projects = new List<ProjectDto>(),
            };
            foreach (var project in projectList.Projects)
            {
                if (project.status == 1)
                {
                    shapedList.Projects.Add(project);
                }
            }
            return shapedList;
        }
    }
}
