using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Resources;
using System;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class Stream : ControllerBase
    {
        IStreamResource _streamResource;
        public Stream(IStreamResource streamResource)
        {
            _streamResource = streamResource;
        }

        // gets stream info
        [HttpGet("{streamName}")]
        [AllowAnonymous]
        public async Task<ActionResult<StreamDto>> GetStreamInfo([FromRoute] string streamName)
        {
            StreamDto stream = null;
            try
            {
                stream = await _streamResource.getStream(streamName);
            }
            catch (Exception)
            {
            }
            return Ok(stream);
        }

        // when user wants to initialize stream this endpoint authenticates this proces
        [HttpPost("authenticate/{streamName}")]
        [AllowAnonymous]
        public async Task<IActionResult> authenticateStream([FromRoute] string streamName, [FromBody] TokenDto token)
        {
            var ok = await _streamResource.authenticate(streamName, token.token);
            return ok ? Ok() : BadRequest();
        }
    }
}
