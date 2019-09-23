using AutoMapper;
using Minenetred.web.Models;
using Minenetred.web.ViewModels;
using Redmine.library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Infrastructure
{
    public class MappingProfile :Profile 
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectListResponse, ProjectsViewModel>()
                .ForMember(dto => dto.Projects, opt => opt.MapFrom(src => src.Projects));

            CreateMap<Activity, ActivityDto>();
            CreateMap<ActivityListResponse, ActivityViewModel>()
                .ForMember(dto => dto.Activities, opt => opt.MapFrom(src => src.Time_Entry_Activities));

            CreateMap<Issue, IssueDto>();
            CreateMap<IssueListResponse, IssueViewModel>();

            CreateMap<TimeEntry, TimeEntryDto>();
            CreateMap<TimeEntryListResponse, TimeEntryViewModel>()
                .ForMember(dto => dto.TimeEntries, opt => opt.MapFrom(src => src.Time_Entries));
        }
    }
}
