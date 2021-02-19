using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Resources;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Home : Controller
    {
        private IHomeResource _homeResource;
        public Home(IHomeResource homeResource)
        {
            _homeResource = homeResource;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> login([FromBody] UserCreditials creditials)
        {
            var user = await _homeResource.Login(creditials.UserName, creditials.Password);
            return Ok(user);
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
              claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(claimsIdentity),
              authProperties);
            return Ok();
        }

        // when user wants to initialize stream this endpoint authenticates this proces
        [HttpPost("authenticate/{streamName}")]
        public async Task<IActionResult> authenticateStream([FromRoute] string streamName, [FromBody] TokenDto token)
        {
            var ok = await _homeResource.authenticate(streamName, token.token);
            return ok ? Ok() : BadRequest();
        }
    }
}
