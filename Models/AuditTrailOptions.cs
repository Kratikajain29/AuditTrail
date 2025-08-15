using System.ComponentModel.DataAnnotations;

namespace AuditTrailAPI.Models
{
    public class AuditTrailOptions
    {
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string DatabaseConnectionString { get; set; } = string.Empty;

        [Range(1, 10)]
        public int MaxRetryAttempts { get; set; } = 3;

        [Required]
        [RegularExpression("^(Debug|Information|Warning|Error|Critical)$")]
        public string LogLevel { get; set; } = "Information";

        [Range(100, 10000)]
        public int MaxAuditLogsPerRequest { get; set; } = 1000;

        [Required]
        public bool EnableDetailedLogging { get; set; } = false;
    }
} 