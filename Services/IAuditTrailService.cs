using AuditTrailAPI.Models;

namespace AuditTrailAPI.Services
{
    public interface IAuditTrailService
    {
        Task<AuditTrailResponse> CreateAuditTrailAsync(AuditTrailRequest request);
        Task<List<AuditTrailResponse>> GetAuditTrailsAsync(string entityName, string? userId = null, DateTime? fromDate = null, DateTime? toDate = null);
    }
} 