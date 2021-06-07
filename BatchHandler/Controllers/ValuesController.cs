namespace BatchHandler.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"Item {id}");
        }
    }
}
