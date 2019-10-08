using AutoMapper;
using Minenetred.web.Infrastructure;
using Minenetred.web.Models;
using Minenetred.web.Services;
using Minenetred.web.Services.Implementations;
using Moq;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Minenetred.web.Test
{
    public class ProjectServiceTest
    {
        private ProjectService _projectSerivce;
        private Mock<Redmine.library.Services.IProjectService> _libraryProjectService;
        private IMapper _mapper;
        public ProjectServiceTest()
        {
            _libraryProjectService = new Mock<Redmine.library.Services.IProjectService>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _projectSerivce = new ProjectService(_libraryProjectService.Object, _mapper);
        }

        [Fact]
        public async Task ShouldReturnOnlyOpenProjectsAsync()
        {
            var openProject = new Mock<Project>();
            var closedProject = new Mock<Project>();

            openProject.Object.Status = 1;
            closedProject.Object.Status = 0;

            var projectList = new List<Project>();
            projectList.Add(openProject.Object);
            projectList.Add(closedProject.Object);

            var FullProjectList = new ProjectListResponse()
            {
                Projects = projectList,
            };
            async Task<ProjectListResponse> AssignResponse()
            {
                await Task.Delay(0);
                return FullProjectList;
            }
            var apiKey = "TestKey";
            _libraryProjectService.Setup(c => c.GetProjectsAsync(apiKey)).Returns(AssignResponse());

            var returnedShappedList = await _projectSerivce.GetOpenProjectsAsync(apiKey);
            foreach (var project in returnedShappedList.Projects)
            {
                Assert.Equal(1, project.Status);
            }
        }
    }
}
