using System.ComponentModel.DataAnnotations;

namespace AuditTrailAPI.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public AuditAction Action { get; set; }
        public string ChangedFieldsJson { get; set; } = string.Empty;
        public string? BeforeObjectJson { get; set; }
        public string? AfterObjectJson { get; set; }
    }
} 