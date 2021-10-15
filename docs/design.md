# Design

This file contains my thoughts on the database and api design. This may also include questions I'm still thinking on and haven't got an answer for just yet.

## Database design

The database must contain these first tables to start off with:

Account

- id : PK
- iban
- name
- type

Transaction

- id : PK
- amount
- fromAccountId : FK
- toAccountId : FK
- description
- date
- categoryId : FK

Category

- id : PK
- description

## thoughts

These are design issues i'm still thinking on or am not sure about yet.

### Duplicate transactions

I need to prevent users from importing duplicate transactions. Rabobank however does not have any unique identifying value to check if the transaction already exists.

I therefore think I will need to implement some kind of hash comparision:

1. Make a hash based on a number of standard fields, and save this with the transaction.
2. Then before adding a new transaction, create the hash and check for a match in the database.

### Rabobank

- Currently only support Rabobank csv files, need to get examples of ING of ABN so I can also process those.

- #### Moet ik balans bij rekening opnemen?

  nee, start balans wordt een losse transactie. Dit betekent wel dat je niet al te veel kan rommelen met transacties van voordat je de start balans ingesteld hebt. Dit heeft namelijk gevolgen voor de geschiedenis van een rekening.

- #### Moet currency bij een transactie opgenomen worden?
  Yes, but this can be added at a later stage. In first instance there is no need for supporting multiple currencies.

### Account types

I've now got 3 basic types, where the last type is questionable. This issue now tho is should I perhaps add sub-account types? For example, with expense accounts you could then also track loans or debts. Or should I use categories for that instead?

- #### Asset accounts

  These are accounts owned by the user. They are used for day-to-day banking, such as paying at the grocery store.

- #### Expense accounts

  These are accounts the user has payed into. So as in the previous example, the account of the grocery store. This way we can also track the amount spent at a specific store for a specific period.

- #### Savings accounts
  I'm not sure yet on if it's usefull for having a specific type for savings. Should this include only users savings accounts, or also others such as pension / share accounts?
