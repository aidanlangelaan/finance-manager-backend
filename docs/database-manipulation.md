# Database Manipulation

The database has been setup using the code-first approach. This means that any alterations (to both structure or data) are managed using EF migrations.

Use the following commands:

- Add migration:

  `dotnet ef --startup-project ./FinanceManager.Api --project ./FinanceManager.Data migrations add {NAME}`

- Remove migration

  `dotnet ef --startup-project ./FinanceManager.Api --project ./FinanceManager.Data migrations remove`

- Update database

  `dotnet ef --startup-project ./FinanceManager.Api --project ./FinanceManager.Data database update`

- Partual database update or rollback to target migration

  `dotnet ef --startup-project ./FinanceManager.Api --project ./FinanceManager.Data database update {TARGET MIGRATION}`
