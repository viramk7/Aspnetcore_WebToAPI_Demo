using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PFM.Models.InputDtos;
using PFM.Services;

namespace PFM.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authenticationService;

        public AuthenticationController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Index()
        {
            var login = new LoginInputDto
            {
                Email = "admin@test.com",
                Password = "Admin@123"
            };

            var result = await _authenticationService.Login(login);
            if (result.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Email),
                    new Claim("access_token", result.AccessToken.Token),
                    new Claim("refresh_token", result.RefreshToken)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
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
            }

            return RedirectToAction("Index", "Home");
        }
    }
}