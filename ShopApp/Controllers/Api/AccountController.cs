using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ShopApp.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using ShopApp.Models.ViewModels;
using ShopApp.Modules.InnerModules;
using ShopApp.Infrastructure;

namespace ShopApp.Controllers.Api
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        public readonly UserManager<User> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly SignInManager<User> signInManager;
        public readonly ILogger<AccountController> log;
        public IVerificationUserAccess verification { get; set; }
        public IRepository repo { get; set; }

        public AccountController(UserManager<User> userMngr, RoleManager<IdentityRole> roleMngr, SignInManager<User> signInMngr, ILogger<AccountController> logger, IVerificationUserAccess vrf, IRepository repository)
        {
            userManager = userMngr;
            roleManager = roleMngr;
            signInManager = signInMngr;
            log = logger;
            verification = vrf;
            repo = repository;
        }

        [HttpPost("check")]
        public async Task<object> Check()
        {
            var b = signInManager.IsSignedIn(HttpContext.User);
            return new { authenticated = b, nickname = userManager.GetUserAsync(HttpContext.User).Result?.UserName };
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Register(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, DateOfRegistration = DateTime.Now};
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInWithClaimsAsync(user, false, new List<Claim>() { new Claim(ClaimTypes.Role, "user") });
                }
                else
                {
                    foreach (var error in result.Errors)
                        log.LogError(error.Description, null);
                }
            }
            return RedirectToAction("MainPage", "Main");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogInModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return RedirectToAction("MainPage", "Main");
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("MainPage", "Main");
        }

        [Authorize]
        [HttpPost("removeUserAccount")]
        public async Task<IActionResult> CanSeeButtonRemove(string login, string password)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var res = await signInManager.CheckPasswordSignInAsync(user, password, false);

            var orders = repo.GetOrdersFromUser(user.Id);
            foreach (var item in orders)
                item.State = OrderState.Cancelled;

            var userProds = repo.GetProductsFromUser(user);
            foreach (var item in userProds)
                item.ProductDeleted = true;

            if (!res.Succeeded)
                return BadRequest();

            await userManager.DeleteAsync(user);

            return Ok();
        }
    }
}
