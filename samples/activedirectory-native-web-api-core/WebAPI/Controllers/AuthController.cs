using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        /// <summary>
        /// GET: api/auth
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Get()
        {
            var isAuthenticated = (bool) HttpContext?.User?.Identity?.IsAuthenticated;
            var user = HttpContext?.User?.Identity;

            return (isAuthenticated)?$"{user.Name} is authenticated!" : "Access denied.";
        }



        /// <summary>
        /// GET: /Auth/Logout
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task LogOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.Authentication.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
