using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class User : Controller
    {
        private IUserResource _userResource;
        public User(IUserResource userResource)
        {
            _userResource = userResource;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Classes.User>> login([FromBody] UserCreditials creditials)
        {
            var user = await _userResource.Login(creditials.UserName, creditials.Password);

            if (user.UserId == null)
            {
                return BadRequest();
            }
            // principal.Identity.Name
            //HttpContext.User.Identities.
            var guid = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Authentication).Select(claim => claim.Value).FirstOrDefault();
            var userToken = await _userResource.getNewUserToken(user.UserId, guid, 365);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Authentication, userToken.Token),
                new Claim(ClaimTypes.Name, creditials.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = userToken.Expiration,
                IsPersistent = true,
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok(user);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<bool>> logout([FromBody] Classes.User user)
        {
            var guid = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Authentication).Select(claim => claim.Value).FirstOrDefault();
            if (guid != null)
            {
                await _userResource.deleteToken(user.UserId, guid);
            }
            await HttpContext.SignOutAsync(
             CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(true);
        }

        [HttpGet()]
        public async Task<ActionResult<Classes.User>> get()
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).Select(claim => claim.Value).FirstOrDefault();
            Classes.User user = null;
            if (userId != null)
            {
                user = await _userResource.getUser(Convert.ToInt32(userId));
            }
            return Ok(user);
        }
    }
}
