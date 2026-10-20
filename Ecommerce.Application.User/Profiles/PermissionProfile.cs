using AutoMapper;
using Ecommerce.Infrastructure.Entities;
using Project.Common.Models.Dtos.ApplicationPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile() {
            CreateMap<ApplicationPermission, ApplicationPermissionDto>().ReverseMap();
            CreateMap<ApplicationPermissionCreateOrUpdateDto, ApplicationPermission>().ReverseMap();
        }
    }
}
