# Transaction System Design

This document outlines the structure, logic, and behavior of the transaction model used in the Personal Finance Manager application.

## 1. Overview

A transaction represents the **movement of money** from one account to another. Every transaction requires:

- A **source account** (money goes from here)
- A **destination account** (money goes to here)
- An **amount**, date, and optional description

Transactions are the foundation of all reporting, budgeting, and balance tracking within the application.

## 2. Transaction Types

There is only **one unified transaction model**, defined by its source and destination:

| From → To               | Interpreted As              |
|-------------------------|-----------------------------|
| Asset → Expense         | Expense (money spent)       |
| Income → Asset          | Income (money received)     |
| Asset → Asset           | Transfer between accounts   |
| Asset → Liability       | Debt repayment              |
| Liability → Asset       | New debt / loan received    |

Note: Some types (e.g. Income, Liability) may be added in the future. For now, the focus is on **Asset → Expense** and **Asset → Asset**.

## 3. Fields

Proposed base model:

```csharp
public class Transaction
{
    public int Id { get; set; }
    public int SourceAccountId { get; set; }
    public int DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int UserId { get; set; }

    public User User { get;set; }
    public Account SourceAccount { get;set; }
    public Account DestinationAccount { get;set; }
    public Category? Category { get;set; }
}

```

## 4. UI Behavior

- Users select **source** and **destination** accounts when creating a transaction.
- Default flow is **Asset → Expense** for simple expense tracking.
- If a new recipient is entered (e.g. store or person), the app can auto-create an **expense account**.
- Transactions are listed chronologically and grouped by date/month.
- Category can be selected manually or auto-suggested based on description or past behavior (planned feature).

## 5. Special Transaction Types (Planned)

### 5.1 Recurring Transactions
- Stored as templates and auto-generated on a defined schedule.
- Supports intervals: daily, weekly, monthly, yearly.
- Users can pause, update, or delete the series.
- Only the **next instance** is created automatically unless otherwise configured.

### 5.2 Split Transactions
- One logical transaction spread across multiple destinations or categories.
- Example: Grocery store payment partly personal, partly business expense.
- UI will allow dynamic row entry per split item.

### 5.3 Scheduled Transactions
- Future-dated transactions, not included in current balances.
- Visible in forecasting or projected reports.
- Can be marked as "cleared" once actually executed.

## 6. Considerations

- **Currency support**: initially, all accounts and transactions are in the same currency. Multi-currency support may be added later.
- **Tags and categories**: useful for budgets and filtering; optional at first, possibly auto-suggested in the future.
- **CSV Import Mapping**: imported transactions must be matched to existing accounts and optionally categorized.
- **Time zones**: dates are stored as UTC, but shown in the user’s local time.

## 7. Example Use Cases

### A. Simple Expense

> You buy groceries at Albert Heijn for €42.

| Field               | Value                      |
|---------------------|----------------------------|
| SourceAccountId     | Checking account           |
| DestinationAccountId| Albert Heijn (expense)     |
| Amount              | 42.00                      |
| Description         | Weekly groceries           |
| Category            | Groceries                  |

### B. Transfer Between Accounts

> You move €500 from savings to checking.

| Field               | Value                      |
|---------------------|----------------------------|
| SourceAccountId     | Savings                    |
| DestinationAccountId| Checking                   |
| Amount              | 500.00                     |
| Description         | Monthly transfer           |
| Category            | Transfer                   |


## 8. Future Improvements

- Add **attachments**, such as receipt uploads per transaction
- Support **change history** for audit/logging purposes
- Allow marking transactions as **cleared**, **pending**, or **reconciled**
- Enable sharing a transaction (e.g. split dinner bill with a contact)
- Track **who entered** or modified the transaction (multi-user audit)