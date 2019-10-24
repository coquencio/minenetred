using AutoMapper;
using Minenetred.Web.Infrastructure;
using Minenetred.Web.Services.Implementations;
using Moq;
using Redmine.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Minenetred.Web.Test
{
    public class ProjectServiceTest
    {
        private ProjectService _projectSerivce;
        private Mock<Redmine.Library.Services.IProjectService> _libraryProjectService;
        private IMapper _mapper;

        public ProjectServiceTest()
        {
            _libraryProjectService = new Mock<Redmine.Library.Services.IProjectService>();
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

            async Task<List<Project>> AssignResponse()
            {
                await Task.Delay(0);
                return projectList;
            }
            var apiKey = "TestKey";
            _libraryProjectService.Setup(c => c.GetProjectsAsync(apiKey)).Returns(AssignResponse());

            var returnedShappedList = await _projectSerivce.GetOpenProjectsAsync(apiKey);
            foreach (var project in returnedShappedList)
            {
                Assert.Equal(1, project.Status);
            }
        }
    }
}