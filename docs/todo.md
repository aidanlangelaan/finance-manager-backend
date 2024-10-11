# Todo list

These are things that still need doing or can be improved on.

## To do

- Create all basic CRUD service and controller calls for Account
- Cleanup previous imported files after x amount of time
- Implement seq for serilog (https://codewithmukesh.com/blog/structured-logging-with-serilog-in-aspnet-core/)

## To improve

- Hash creation and checks for transactions to prevent the creation of duplicate transactions
  - When importing show user how many rows imported successful, how many duplicate and how many failed
- Authentication
  - Check automapper configs
  - Update user on email confirmation (email conf, unlock, security hash?)
  - Check on expiration of tokens (configuration in startup)
  - Implement password reset
  - Implement logout
  - Handle case where user hasn't confirmed emailaddress on time (before token expiration)
  - rate limiting for authentication endpoints or in general
- Category matching
  - Figure out a more accurate way to identify similar transactions with fuzzy matching of description
  - Make the fuzzy method usable from different locations, e.g. de import service
