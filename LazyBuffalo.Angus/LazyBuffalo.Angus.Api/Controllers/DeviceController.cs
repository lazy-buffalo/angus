using LazyBuffalo.Angus.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LazyBuffalo.Angus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        // POST api/device
        [HttpPost]
        public void Post([FromBody] Rootobject rootobject)
        {
        }
    }
}
