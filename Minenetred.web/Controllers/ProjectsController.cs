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

namespace Minenetred.web.Controllers
{
    
    public class ProjectsController : Controller
    {
        public Projects _apiProjects { get; set; }
        private MapperConfiguration _config { get; set; }
        private IMapper _mapper { get; set; }
        public ProjectsController()
        {
            _apiProjects = new Projects();
            _config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Project, ProjectDto>();
                cfg.CreateMap<ProjectsContent, ProjectsViewModel>()
                .ForMember(dto => dto.Projects, opt => opt.MapFrom(src => src.Projects));
            });
            _mapper = _config.CreateMapper();
        }

        [Route("Projects")]
        [HttpGet]
        public async Task<ActionResult<ProjectsViewModel>> ProjectsAsync()
        {
            var apiContent = await _apiProjects.GetProjects();
            var projectsList = _mapper.Map<ProjectsContent, ProjectsViewModel>(apiContent);
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
