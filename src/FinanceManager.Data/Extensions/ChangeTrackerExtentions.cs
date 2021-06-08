using System;
using System.Collections.Generic;
using System.Text;
using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FinanceManager.Data.Extensions
{
	public static class ChangeTrackerExtensions
	{
		public static void ApplyAuditInformation(this ChangeTracker changeTracker)
		{
			foreach (var entry in changeTracker.Entries())
			{
				if (!(entry.Entity is AuditableEntity auditableEntity)) continue;

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
				}
			}
		}
	}
}
