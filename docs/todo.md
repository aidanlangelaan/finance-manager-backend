# Todo list

These are things that still need doing or can be improved on.

## To do

- Create all basic CRUD service and controller calls for Account
- Generic error handling to prevent uncaught 500 errors being returned

## To improve

- hash creation and checks for transactions to prevent the creation of duplicate transactions.
- Authentication
  - Upgrade identity packages to use the latest .net 8 versions 
  - Remove password from registration and replace with email confirmation with temp password
  - Implement registration confirmation
  - Implement logout
- Category matching
  - Figure out a more accurate way to identify similar transactions with fuzzy matching of description
