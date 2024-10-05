# Todo list

These are things that still need doing or can be improved on.

## To do

- Create all basic CRUD service and controller calls for Account
- Generic error handling to prevent uncaught 500 errors being returned
- Cleanup previous imported files after x amount of time
- Implement seq for serilog (https://codewithmukesh.com/blog/structured-logging-with-serilog-in-aspnet-core/)

## To improve

- Hash creation and checks for transactions to prevent the creation of duplicate transactions
  - When importing show user how many rows imported successful, how many duplicate and how many failed
- Authentication
  - Upgrade identity packages to use the latest .net 8 versions 
  - Remove password from registration and replace with email confirmation with temp password
  - Implement registration confirmation
  - Implement logout
- Category matching
  - Figure out a more accurate way to identify similar transactions with fuzzy matching of description
  - Make the fuzzy method usable from different locations, e.g. de import service
