# Todo list

These are things that still need doing or can be improved on.

## To do

- Implement authentication and authorization
  - Implement authentication on methods
  - Implement accesstoken refreshing
  - Remove password from registration
  - Implement registration confirmation
- Check if transaction and accounts are unique when importing records.
- Create all basic CRUD service and controller calls for Account, Transaction and Categories.

## To improves

- Import needs improving to make it perform better and handle larger datasets

  - hash creation and checks for transactions to prevent the creation of duplicate transactions.

  - Bached processing of both files and records within. It now handles everyting the 1 after the other which makes it take a looooong time to import e.g. 2500 records. This needs to be done in transactions and need to think of a way wait on saving everything till batch has completed instead for each account and transaction.

  - User can upload file(s) to be processed. These are then stored in a temp location until a scheduled job (hangfire?) picks the file up and processes it. That way the user doesn't have to wait. This does mean we have to show the status of the imports somewhere...
