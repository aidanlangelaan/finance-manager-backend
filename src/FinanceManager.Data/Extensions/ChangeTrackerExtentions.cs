using System;
using FinanceManager.Data.Entities;
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
		}
	}
}