using Ecommerce.Application.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Project.Common.Libraries;
using Project.Common.Models;
using Project.Common.Models.Dtos.Auth;
using Project.Common.Models.ViewModels.Auth;

namespace Ecommerce.CMS.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthServcie _authServcie;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AuthController(ILogger<AuthController> logger, IAuthServcie authServcie, IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _authServcie = authServcie;
            _localizer = localizer;
        }

        [HttpGet("dang-nhap-he-thong")]
        public IActionResult Index(string returnUrl = "/", string error = null)
        {
            var viewModel = new SignInViewModel
            {
                Id = 0,
                Password = string.Empty,
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost("dang-nhap-he-thong")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var signModel = new SignInDto
            {
                Id = model.Id,
                Password = model.Password
            };

            var result = await _authServcie.ProcessSignInAsync(signModel);

            if (result.IsSuccess)
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, _localizer[result.Message]);
                return View(model);
            }
        }
     }
}
