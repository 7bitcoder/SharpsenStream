using Microsoft.AspNetCore.Mvc;
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
    }
}
