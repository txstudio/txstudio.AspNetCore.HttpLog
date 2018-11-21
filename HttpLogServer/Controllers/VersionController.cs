using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HttpLogServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class VersionController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return ("AspNetCore.HttpLog Demo App");
        }
    }
}
