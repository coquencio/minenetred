using AutoMapper;
using Minenetred.Web.Models;
using Redmine.Library.Models;

namespace Minenetred.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();

            CreateMap<Activity, ActivityDto>();

            CreateMap<Issue, IssueDto>();

            CreateMap<TimeEntry, Minenetred.Web.Models.TimeEntryDto>();
        }
    }
}