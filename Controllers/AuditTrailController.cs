using Microsoft.AspNetCore.Mvc;
using AuditTrailAPI.Models;
using AuditTrailAPI.Services;

namespace AuditTrailAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuditTrailController : ControllerBase
    {
        private readonly IAuditTrailService _auditTrailService;
        private readonly ILogger<AuditTrailController> _logger;

        public AuditTrailController(IAuditTrailService auditTrailService, ILogger<AuditTrailController> logger)
        {
            _auditTrailService = auditTrailService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<AuditTrailResponse>> CreateAuditTrail([FromBody] AuditTrailRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request cannot be null");
                }

                if (string.IsNullOrWhiteSpace(request.EntityName))
                {
                    return BadRequest("Entity name is mandatory");
                }

                if (string.IsNullOrWhiteSpace(request.UserId))
                {
                    return BadRequest("User ID is mandatory");
                }

                var result = await _auditTrailService.CreateAuditTrailAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller error: creating audit trail");
                return StatusCode(500, "Error occurred while creating the trail");
            }
        }

        [HttpGet("{entityName}")]
        public async Task<ActionResult<List<AuditTrailResponse>>> GetAuditTrails(
            string entityName,
            [FromQuery] string? userId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityName))
                {
                    return BadRequest("Entity name is mandatory");
                }

                var result = await _auditTrailService.GetAuditTrailsAsync(entityName, userId, fromDate, toDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller error: retrieving audit trails for {EntityName}", entityName);
                return StatusCode(500, "Error occurred while retrieving the audit trails");
            }
        }

        [HttpGet("health")]
        public ActionResult<string> Health()
        {
            return Ok("AuditTrail API is running");
        }
    }
} 