using Ecommerce.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Project.Common.Servcies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Servcies
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditSaveChangesInterceptor(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context == null)
            {
                return base.SavingChangesAsync(
                    eventData,
                    result,
                    cancellationToken);
            }
            UpdateAuditableEntities(context);
            CreateAuditLogs(context);

            return base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }
        private void UpdateAuditableEntities(DbContext context)
        {
            var userId = _currentUserService.UserId ?? "System";
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // Kiểm tra nếu entity có các property UserId và IpAddress (có thể dùng Interface chung ví dụ: IAuditable)
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var userIdProp = entry.Property("ChangedBy");
                    var ipAddressProp = entry.Property("IpAddress");

                    if (userIdProp != null && entry.State == EntityState.Added) // Thường chỉ set UserId lúc Add, hoặc cập nhật cả Modified tùy logic
                    {
                        userIdProp.CurrentValue = userId;
                    }

                    if (ipAddressProp != null)
                    {
                        ipAddressProp.CurrentValue = ipAddress;
                    }
                }
            }
        }
        private void CreateAuditLogs(DbContext context)
        {
            var logs = new List<AuditLog>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // Không audit chính AuditLog
                if (entry.Entity is AuditLog)
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            var log = CreateAddedLog(entry);

                            if (log != null)
                                logs.Add(log);

                            break;
                        }

                    case EntityState.Modified:
                        {
                            var log = CreateModifiedLog(entry);

                            if (log != null)
                                logs.Add(log);

                            break;
                        }

                    case EntityState.Deleted:
                        {
                            var log = CreateDeletedLog(entry);

                            if (log != null)
                                logs.Add(log);

                            break;
                        }
                }
            }

            if (logs.Count > 0)
            {
                context.Set<AuditLog>().AddRange(logs);
            }
        }

        private AuditLog CreateAddedLog(EntityEntry entry)
        {
            return new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = GetEntityId(entry),
                Action = "INSERT",
                ChangedAt = DateTime.UtcNow,

                OldValues = null,

                NewValues = JsonSerializer.Serialize(
                    GetCurrentValues(entry))
            };
        }

        private AuditLog? CreateModifiedLog(EntityEntry entry)
        {
            var oldValues = new Dictionary<string, object?>();
            var newValues = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                // Chỉ lấy những property thực sự thay đổi
                if (!property.IsModified)
                    continue;

                // Không audit field nhạy cảm
                if (IsSensitiveProperty(property.Metadata.Name))
                    continue;

                oldValues[property.Metadata.Name] =
                    property.OriginalValue;

                newValues[property.Metadata.Name] =
                    property.CurrentValue;
            }

            // UPDATE nhưng không có field nào thực sự thay đổi
            if (oldValues.Count == 0)
                return null;

            return new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = GetEntityId(entry),
                Action = "UPDATE",
                ChangedAt = DateTime.UtcNow,

                OldValues = JsonSerializer.Serialize(oldValues),

                NewValues = JsonSerializer.Serialize(newValues)
            };
        }

        private AuditLog CreateDeletedLog(EntityEntry entry)
        {
            return new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = GetEntityId(entry),
                Action = "DELETE",
                ChangedAt = DateTime.UtcNow,

                OldValues = JsonSerializer.Serialize(
                    GetOriginalValues(entry)),

                NewValues = null
            };
        }

        private string GetEntityId(EntityEntry entry)
        {
            var primaryKey = entry.Metadata.FindPrimaryKey();

            if (primaryKey == null)
                return string.Empty;

            var values = primaryKey.Properties
                .Select(property =>
                    entry.Property(property.Name).CurrentValue)
                .ToArray();

            return string.Join(",", values);
        }

        private Dictionary<string, object?> GetCurrentValues(
            EntityEntry entry)
        {
            var values = new Dictionary<string, object?>();

            foreach (var property in entry.CurrentValues.Properties)
            {
                if (IsSensitiveProperty(property.Name))
                    continue;

                values[property.Name] =
                    entry.CurrentValues[property];
            }

            return values;
        }

        private Dictionary<string, object?> GetOriginalValues(
            EntityEntry entry)
        {
            var values = new Dictionary<string, object?>();

            foreach (var property in entry.OriginalValues.Properties)
            {
                if (IsSensitiveProperty(property.Name))
                    continue;

                values[property.Name] =
                    entry.OriginalValues[property];
            }

            return values;
        }

        private bool IsSensitiveProperty(string propertyName)
        {
            return propertyName.Equals(
                       "PasswordHash",
                       StringComparison.OrdinalIgnoreCase)
                   || propertyName.Equals(
                       "SecurityStamp",
                       StringComparison.OrdinalIgnoreCase)
                   || propertyName.Equals(
                       "ConcurrencyStamp",
                       StringComparison.OrdinalIgnoreCase)
                   || propertyName.Contains(
                       "Token",
                       StringComparison.OrdinalIgnoreCase)
                   || propertyName.Contains(
                       "Secret",
                       StringComparison.OrdinalIgnoreCase);
        }
    }
}
