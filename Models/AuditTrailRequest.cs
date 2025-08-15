using System.Text.Json;

namespace AuditTrailAPI.Models
{
    public class AuditTrailRequest
    {
        public JsonElement? BeforeObject { get; set; }
        public JsonElement? AfterObject { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public AuditAction Action { get; set; }
    }
} 