using System.Text.Json;
using AuditTrailAPI.Data;
using AuditTrailAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuditTrailAPI.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly AuditDbContext _context;
        private readonly ILogger<AuditTrailService> _logger;
        private readonly AuditTrailOptions _options;

        public AuditTrailService(AuditDbContext context, ILogger<AuditTrailService> logger, IOptions<AuditTrailOptions> options)
        {
            _context = context;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<AuditTrailResponse> CreateAuditTrailAsync(AuditTrailRequest request)
        {
            try
            {
                var hasChanges = !AreObjectsEqual(request.BeforeObject, request.AfterObject);
                
                var auditTrailResponse = new AuditTrailResponse
                {
                    ChangedFields = hasChanges ? GetChangedFields(request.BeforeObject, request.AfterObject, request.Action) : new List<ChangedField>(),
                    Metadata = new AuditMetadata
                    {
                        Timestamp = DateTime.UtcNow,
                        UserId = request.UserId,
                        EntityName = request.EntityName,
                        Action = request.Action
                    }
                };

                var auditLog = new AuditLog
                {
                    Timestamp = auditTrailResponse.Metadata.Timestamp,
                    UserId = auditTrailResponse.Metadata.UserId,
                    EntityName = auditTrailResponse.Metadata.EntityName,
                    Action = auditTrailResponse.Metadata.Action,
                    ChangedFieldsJson = JsonSerializer.Serialize(auditTrailResponse.ChangedFields),
                    BeforeObjectJson = request.BeforeObject?.ToString(),
                    AfterObjectJson = request.AfterObject?.ToString()
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();

                if (_options.EnableDetailedLogging)
                {
                    _logger.LogInformation("Audit trail created: {EntityName} by {UserId} with {ChangeCount} changes", 
                        request.EntityName, request.UserId, auditTrailResponse.ChangedFields.Count);
                }
                else
                {
                    _logger.LogInformation("Audit trail created: {EntityName} by {UserId}", 
                        request.EntityName, request.UserId);
                }

                return auditTrailResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating audit trail: {EntityName}", request.EntityName);
                throw;
            }
        }

        public async Task<List<AuditTrailResponse>> GetAuditTrailsAsync(string entityName, string? userId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var query = _context.AuditLogs.AsQueryable();

                query = query.Where(a => a.EntityName == entityName);

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(a => a.UserId == userId);
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp <= toDate.Value);
                }

                var auditLogs = await query
                    .OrderByDescending(a => a.Timestamp)
                    .Take(_options.MaxAuditLogsPerRequest)
                    .ToListAsync();

                var responses = new List<AuditTrailResponse>();

                foreach (var log in auditLogs)
                {
                    var changedFields = JsonSerializer.Deserialize<List<ChangedField>>(log.ChangedFieldsJson) ?? new List<ChangedField>();
                    
                    responses.Add(new AuditTrailResponse
                    {
                        ChangedFields = changedFields,
                        Metadata = new AuditMetadata
                        {
                            Timestamp = log.Timestamp,
                            UserId = log.UserId,
                            EntityName = log.EntityName,
                            Action = log.Action
                        }
                    });
                }

                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit trails: {EntityName}", entityName);
                throw;
            }
        }

        private bool AreObjectsEqual(JsonElement? before, JsonElement? after)
        {
            if (before == null && after == null) return true;
            if (before == null || after == null) return false;
            
            return before.Value.ToString() == after.Value.ToString();
        }

        private List<ChangedField> GetChangedFields(JsonElement? before, JsonElement? after, AuditAction action)
        {
            var changedFields = new List<ChangedField>();

            switch (action)
            {
                case AuditAction.Created:
                    if (after.HasValue)
                    {
                        changedFields.Add(new ChangedField
                        {
                            FieldName = "All Fields",
                            OldValue = null,
                            NewValue = "Object Created"
                        });
                    }
                    break;

                case AuditAction.Updated:
                    if (before.HasValue && after.HasValue)
                    {
                        changedFields.Add(new ChangedField
                        {
                            FieldName = "Object",
                            OldValue = "Previous State",
                            NewValue = "Current State"
                        });
                    }
                    break;

                case AuditAction.Deleted:
                    if (before.HasValue)
                    {
                        changedFields.Add(new ChangedField
                        {
                            FieldName = "All Fields",
                            OldValue = "Object Deleted",
                            NewValue = null
                        });
                    }
                    break;
            }

            return changedFields;
        }
    }
} 