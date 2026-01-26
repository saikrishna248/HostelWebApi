using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception");
        }
    }
}
