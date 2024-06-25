using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        public IActionResult CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? reponse = await _couponService.CreateCouponsAsync(model);
                if(reponse != null && reponse.IsSuccess)
                {
                    TempData["success"] = "Created Succesfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = reponse?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? reponse = await _couponService.DeleteCouponsAsync(couponId);
            if (reponse != null && reponse.IsSuccess)
            {
                TempData["success"] = "Deleted Succesfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = reponse?.Message;
            }
            return RedirectToAction(nameof(CouponIndex));
        }
        
    }
}
