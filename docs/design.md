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

### Rabobank

- Currently only support Rabobank csv files, need to get examples of ING of ABN so I can also process those.

- Moet ik balans bij rekening opnemen?

  - nee, start balans wordt een losse transactie. Dit betekent wel dat je niet al te veel kan rommelen met transacties van voordat je de start balans ingesteld hebt. Dit heeft
    namelijk gevolgen voor de geschiedenis van een rekening.

- Moet currency bij een transactie opgenomen worden?
  - not sure yet...

### Account types

I need to do more thinking on the account types, I've now got 3 basic types but my feeling it that these aren't correct or can't capture all different account types.

Asset

- payment
- savings
- shared?
- cash?

Expense

- loan?
- debts?
