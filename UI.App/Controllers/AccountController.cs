using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using UI.App.Models;
using UI.App.Service;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace UI.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IApiService apiService;

        public AccountController(ILogger<AccountController> logger, IApiService apiService)
        {
            _logger = logger;
            this.apiService = apiService;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginVm { ReturnUrl = returnUrl});
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (ModelState.IsValid)
            {
                var token = "";
                try
                {
                    token = await apiService.TryLogin(model.EmailAddress, model.Password);
                }
                catch (Exception)
                {
                    foreach (var err in apiService.GetErrors())
                        ModelState.AddModelError(string.Empty, err);
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Thumbprint, token),
                    new Claim(ClaimTypes.Email, model.EmailAddress),
                    new Claim(ClaimTypes.Name, model.EmailAddress),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = model.RememberMe,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("User {Email} logged in at {Time}.",
                    model.EmailAddress, DateTime.UtcNow);

                model.ReturnUrl = model.ReturnUrl ?? "~/";
                return LocalRedirect(model.ReturnUrl);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}