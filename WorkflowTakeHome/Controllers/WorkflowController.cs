using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WorkflowTakeHome.Services;

namespace WorkflowTakeHome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowStagingService _stagingService;

        public WorkflowController(IWorkflowStagingService stagingService)
        {
            _stagingService = stagingService;
        }

        [HttpPost("stage")]
        public IActionResult StageData([FromQuery] string sourceTool, [FromBody] JsonElement payload)
        {
            try
            {
                var staged = _stagingService.Stage(sourceTool, payload);
                return Ok(staged);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}