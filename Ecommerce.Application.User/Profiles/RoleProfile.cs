using AutoMapper;
using Ecommerce.Infrastructure.Entities;
using Project.Common.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.User.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile(){
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, RoleCreateOrUpdateDto>().ReverseMap();
        }
    }
}
