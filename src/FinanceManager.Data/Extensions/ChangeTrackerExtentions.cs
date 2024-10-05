using System;
using FinanceManager.Data.Entities;
using FinanceManager.Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FinanceManager.Data.Extensions;

public static class ChangeTrackerExtensions
{
	public static void ApplyAuditInformation(this ChangeTracker changeTracker)
	{
		foreach (var entry in changeTracker.Entries())
		{
			if (entry.Entity is not AuditableEntity auditableEntity) continue;

			var now = DateTime.UtcNow;
			switch (entry.State)
			{
				case EntityState.Added:
					auditableEntity.CreatedOnAt = now;
					auditableEntity.UpdatedOnAt = now;
					break;
				
				case EntityState.Modified:
					auditableEntity.UpdatedOnAt = now;
					break;
				
				case EntityState.Detached:
				case EntityState.Unchanged:
				case EntityState.Deleted:
				default:
					// do nothing
				break;
			}

			// update the hash
			if (auditableEntity.HasProperty("HashProperties"))
			{
				var hashProperties = auditableEntity.GetPropertyValue<object[]>("HashProperties");
				auditableEntity.Hash = HashUtils.GetHash(hashProperties!);
			}
			else
			{
				auditableEntity.Hash = HashUtils.GetHash(auditableEntity);
			}
		}
	}
}