using Microsoft.AspNetCore.Mvc;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Resources;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Stream : ControllerBase
    {
        IStreamResource _streamResource;
        public Stream(IStreamResource streamResource)
        {
            _streamResource = streamResource;
        }

        // gets stream info
        [HttpGet("{streamName}")]
        public async Task<IActionResult> GetStreamInfo([FromRoute] string streamName)
        {
            var stream = await _streamResource.getStream(streamName);
            return Ok(stream);
        }

        // when user wants to initialize stream this endpoint authenticates this proces
        [HttpPost("{streamName}/authenticate")]
        public async Task<IActionResult> authenticateStream([FromRoute] string streamName, [FromBody] TokenDto token)
        {
            var ok = await _streamResource.authenticate(streamName, token.token);
            return ok ? Ok() : BadRequest();
        }
    }
}
