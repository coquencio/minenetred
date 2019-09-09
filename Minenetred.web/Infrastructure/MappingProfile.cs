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
        }
    }
}
