using AutoMapper;
using Project.Common.Models.Dtos.ApplicationPermission;
using Project.Common.Models.ViewModels.ApplicationPermission;

namespace Ecommerce.CMS.Common.Mapping
{
    public class PermissionViewProfile : Profile
    {
        public PermissionViewProfile()
        {
            CreateMap<PermissionSearchViewModel, ApplicationPermissionQuery>().ReverseMap();
            CreateMap<ApplicationPermissionDto, ApplicationPermissionViewModel>().ReverseMap();
            CreateMap<ApplicationPermissionDto, ApplicationPermissionUpsertViewModel>().ReverseMap();
            CreateMap<ApplicationPermissionCreateOrUpdateDto, ApplicationPermissionUpsertViewModel>().ReverseMap();
        }
    }
}
