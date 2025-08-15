namespace AuditTrailAPI.Models
{
    public class AuditTrailResponse
    {
        public List<ChangedField> ChangedFields { get; set; } = new();
        public AuditMetadata Metadata { get; set; } = new();
    }

    public class ChangedField
    {
        public string FieldName { get; set; } = string.Empty;
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
    }

    public class AuditMetadata
    {
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public AuditAction Action { get; set; }
    }
} 