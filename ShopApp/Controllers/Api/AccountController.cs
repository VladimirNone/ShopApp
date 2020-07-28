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

namespace ShopApp.Controllers.Api
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        public readonly UserManager<User> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly SignInManager<User> signInManager;
        public readonly ILogger<AccountController> log;

        public AccountController(UserManager<User> userMngr, RoleManager<IdentityRole> roleMngr, SignInManager<User> signInMngr, ILogger<AccountController> logger)
        {
            userManager = userMngr;
            roleManager = roleMngr;
            signInManager = signInMngr;
            log = logger;

        }
        [HttpPost("check")]
        public object Check()
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
                    //return RedirectToAction("MainPage", "Main");
                }
            }
            return RedirectToAction("MainPage", "Main");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("MainPage", "Main");
        }

    }
}
