using AutoMapper;
using Ecommerce.Application.User.Interfaces;
using Ecommerce.CMS.Common.Extensions;
using Ecommerce.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Enum.Entities;
using Project.Common.Models;
using Project.Common.Models.Dtos.ApplicationPermission;
using Project.Common.Models.ViewModels.ApplicationPermission;

namespace Ecommerce.CMS.Controllers
{
    [Authorize]
    public class ApplicationPermissionController : Controller
    {
        private readonly ILogger<ApplicationPermissionController> _logger;
        private readonly IMapper _mapper;
        private readonly IRolePermissionService _permissionService;
        public ApplicationPermissionController(ILogger<ApplicationPermissionController> logger, IRolePermissionService permissionService,
            IMapper mapper)
        {
            _logger = logger;
            _permissionService = permissionService;
            _mapper = mapper;
        }

        [HasPermission(PermissionKey.system_permission_key_view)]
        [HttpGet("quan-ly-quyen-he-thong")]
        public IActionResult Index()
        {
            var vm = new PermissionSearchViewModel();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetTableData([FromQuery] PermissionSearchViewModel permissionSearchViewModel, CancellationToken cancellationToken)
        {
            // 1. Gọi Service lấy dữ liệu phân trang (đã map sang ApplicationPermissionViewModel)
            var query = _mapper.Map<ApplicationPermissionQuery>(permissionSearchViewModel);
            var result = await _permissionService.GetApplicationPermissionAsync(query, cancellationToken);
            var viewModels = _mapper.Map<List<ApplicationPermissionViewModel>>(result.Items);
            var pageResult = new PagedResult<ApplicationPermissionViewModel>()
            {
                Items = viewModels,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };
            // 2. Trả về PartialView kèm theo dữ liệu phân trang
            return PartialView("_PermissionTable", pageResult);
        }

        [HttpGet("quan-ly-key-quen")]
        public async Task<IActionResult> Upsert(int? id, CancellationToken cancellationToken)
        {
            var vm = new ApplicationPermissionUpsertViewModel();
            if (id.HasValue)
            {
                var permissionResult = await _permissionService.GetApplicationPermissionByIdAsync(id.Value, cancellationToken);
                if (permissionResult.IsSuccess)
                {
                    vm = _mapper.Map<ApplicationPermissionUpsertViewModel>(permissionResult.Data);
                }
            }
            return View(vm);
        }

        [HttpGet("them-moi-key-quyen")]
        public async Task<IActionResult> CreateOrUpdate([FromQuery] ApplicationPermissionUpsertViewModel viewModel, CancellationToken cancellationToken){
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var permissionDto = _mapper.Map<ApplicationPermissionCreateOrUpdateDto>(viewModel);
            var result = await _permissionService.CreateOrUpdateApplicationPermissionAsync(permissionDto, cancellationToken);
            
            if (result.IsSuccess)
            {
               viewModel = _mapper.Map<ApplicationPermissionUpsertViewModel>(result.Data);
            }
            viewModel.Message = result.Message;
            viewModel.IsAction = true;
            return View(viewModel);
        }

    }
}
