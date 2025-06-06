# Account Types Design

This document describes the account model used in the Personal Finance Manager application, its purpose, categories, and how they are represented in the database and user interface.

## Overview

The system revolves around **accounts** that represent the source and destination of transactions. Each transaction must have a **source account** and a **destination account**.

Accounts are divided into **two main categories**:

- **Assets** – accounts you own or control
- **Expenses** – accounts where money is spent

## 1. Asset Accounts

Asset accounts represent money that is in your control. They typically hold positive balances.

### Examples:
- Personal checking account
- Savings account
- Cash wallet
- Credit card account (may have negative balance)

### Characteristics:
| Field            | Value                                            |
|------------------|--------------------------------------------------|
| `type`           | `asset`                                          |
| `includedInNet`  | `true` (used to calculate net worth)             |
| `canTransferFrom`| `true`                                           |
| `canTransferTo`  | `true`                                           |

## 2. Expense Accounts

Expense accounts represent where you spend money, without owning the account itself. These accounts are typically the **targets** of your spending.

### Examples:
- Albert Heijn supermarket
- Your landlord
- A friend you paid back
- Netflix

### Characteristics:
| Field            | Value                                            |
|------------------|--------------------------------------------------|
| `type`           | `expense`                                        |
| `includedInNet`  | `false`                                          |
| `canTransferFrom`| `false`                                          |
| `canTransferTo`  | `true` (as payment targets)                      |

## 3. Planned Account Types (future)

These types may be added later for more advanced categorization and reporting:

### 3.1 Income
Represents sources of income.

- Employer
- Government allowance
- Investment returns

### 3.2 Liability
Represents debts or credit facilities.

- Loan from a friend
- Credit card
- Mortgage

### 3.3 Equity
Opening balance, retained earnings, manual adjustments, etc.

## 4. Data Model

Proposed base entity for accounts:

```csharp
public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AccountType Type { get; set; } // e.g., Asset, Expense
    public string? Description { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IncludedInNetWorth { get; set; }
    public bool CanTransferFrom { get; set; }
    public bool CanTransferTo { get; set; }
    public int UserId { get; set; }
}

public enum AccountType
{
    Asset,
    Expense,
    Income,     // future
    Liability,  // future
    Equity      // future
}

```

## 5. UI Behavior

- Only **asset** accounts are selectable when entering the "source" of a transaction.
- Only **asset** and **expense** accounts are selectable when entering the "target" of a transaction.
- Expense accounts may optionally be **automatically created** when spending money to a new, unrecognized destination (e.g. a store or individual).
- Users can view accounts grouped by type (e.g. tabs or filters: Assets / Expenses).

## 6. Future Considerations

- Allow users to **archive** or **deactivate** accounts that are no longer in use.
- Add support for **shared accounts**, such as a joint bank account or household budget.
- Enable users to **group accounts** by category or tag (e.g. "Daily", "Emergency", "Investment").
- Add **account color coding** or icons for easier recognition.
- Support **virtual accounts** to group multiple real-world accounts under one umbrella (e.g. "Travel Fund").