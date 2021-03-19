using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Jwt;
using SharpsenStreamBackend.Resources;
using SharpsenStreamBackend.Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class User : Controller
    {
        private IUserResource _userResource;
        private IJwtAuthManager _jwtAuthManager;
        public User(IUserResource userResource, IJwtAuthManager jwtAuthManager)
        {
            _userResource = userResource;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResult>> login([FromBody] UserCreditials creditials)
        {
            var user = await _userResource.Login(creditials.UserName, creditials.Password);

            if (user.UserId == 0)
            {
                return BadRequest();
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, creditials.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(user.UserId, creditials.UserName, claims, DateTime.Now);
            
            return Ok(new LoginResult
            {
                Username = user.Username,
                AvatarFilePath = user.AvatarFilePath,
                Color = user.Color,
                Email = user.Email,
                UserId = user.UserId,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
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
