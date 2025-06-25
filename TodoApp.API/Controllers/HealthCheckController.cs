using Microsoft.AspNetCore.Mvc;

namespace TodoApp.API.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "API is running", timestamp = DateTime.UtcNow });
        }
    }
}