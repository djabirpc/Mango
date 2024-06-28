using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>() 
            {
                new SelectListItem{Text =SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text =SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            ResponseDto response = await _authService.RegisterAsync(model);
            ResponseDto assignRole;

            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = SD.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(model);
                if (assignRole != null && assignRole.IsSuccess) 
                {
                    TempData["success"] = "Registration Success";
                    return RedirectToAction(nameof(Login));
                }
            }


            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text =SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text =SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            LoginRequestDto loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }
    }
}
