using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redmine.library;
using Minenetred.web.Models;
using Redmine.library.Models;
using Minenetred.web.ViewModels;
using AutoMapper;
using System.Net.Http;
using Redmine.library.Services;

namespace Minenetred.web.Controllers
{
    
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        public ProjectsController(IMapper mapper, IProjectService service)
        {
            _mapper = mapper;
            _projectService = service;
            
        }

        [Route("/")]
        [HttpGet]
        public async Task<ActionResult<ProjectsViewModel>> GetProjectsAsync()
        {
            var apiContent = await _projectService.GetProjectsAsync("Try your own key");
            var projectsList = _mapper.Map<ProjectListResponse, ProjectsViewModel>(apiContent);
            var shapedList = new ProjectsViewModel()
            {
                Projects = new List<ProjectDto>(),
            };
            foreach (var project in projectsList.Projects)
            {
                if (project.status == 1)
                    shapedList.Projects.Add(project);

            }
            return shapedList;
        }
    }
}
