# Transaction Categorization

This document outlines how transactions are categorized to support budgeting, reporting, and analytics.

## 1. What is a Category?

A **category** is a label assigned to a transaction that describes the type of spending or income. It is separate from the "destination account", which may refer to a specific payee (e.g. Albert Heijn).

Examples:

- Groceries
- Rent
- Utilities
- Salary
- Entertainment

## 2. Category Sources

Categories can be:

- **Manually selected** during transaction entry
- **Inferred from destination account** (e.g. "Netflix" always → "Entertainment")
- **Auto-suggested** using rules or historical patterns

## 3. Category Assignment Flow

1. Check if the user manually selected a category.
2. Check if the destination account has a default category.
3. Optionally apply rules (e.g. based on keywords in the description).
4. Leave empty if no match is found.

## 4. Managing Categories

Users can:

- Create, rename, and delete categories
- Assign a default category to an expense account
- Merge two or more categories

## 5. Use in Reporting

Categories are used to:

- Group transactions in overviews
- Calculate totals per category per month
- Compare actual spending to budgets
- Show category-based charts in dashboards

## 6. Future Plans

- Allow nested categories (e.g. "Food > Groceries", "Food > Dining out")
- Add tags alongside or instead of categories
- Allow category rules during CSV import
- Show category suggestions while typing or importing

## 7. Implementation Notes

- Categories are stored in a separate database table, per user.
- During transaction import or creation, the system will:
  1. Check if a manual category was provided.
  2. Attempt to infer based on the destination account's default category.
  3. Optionally apply custom rules (future).
  4. If all else fails and a default category is configured for the user, use that.
