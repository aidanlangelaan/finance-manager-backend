# Transaction Import

This document describes how the application handles importing transactions from external sources such as bank exports. The import system is designed to be flexible, user-friendly, and scalable for both small and large datasets.

## 1. Supported Banks

The application supports importing CSV exports from major Dutch banks:

### ✅ Built-in Templates

| Bank        | Notes                                             |
|-------------|---------------------------------------------------|
| Rabobank    | Uses semicolon `;` delimiter, European date style |
| ING         | Includes "Af/Bij" column for debit/credit         |
| ABN AMRO    | Often includes separate description fields         |

Each bank format has its own:
- Column names
- Field ordering
- Date formats
- Delimiters (`;`, `,`, tab)

The system will initially allow users to **manually select** the appropriate template. In the future the system may **auto-detect** the format.

## 2. Custom Import Mapping

In addition to built-in templates, users can define their own **import mapping profiles**.

### 2.1 Mapping UI Features

- Upload sample CSV file
- Select delimiter (`;`, `,`, tab)
- Assign CSV columns to internal fields:
  - Date
  - Description
  - Amount
  - Type / Direction (optional)
  - External Account / IBAN
- Choose date format and decimal separator
- Save mapping as a **profile** for reuse

Mapping profiles are **per-user** and stored in the database.

## 3. Import Workflow

1. **Upload** a CSV file
2. **Select template** or apply saved mapping profile
3. **Adjust mapping** (if needed)
4. **Preview** the first 20–50 rows
5. **Select source account**
6. **Submit for background processing**

The import is then handled in the background using a **Hangfire job**.

## 4. Column Mapping Fields

| Internal Field     | Description                                        |
|--------------------|----------------------------------------------------|
| `Date`             | Transaction date (e.g. `dd-MM-yyyy`)               |
| `Description`      | Transaction description or memo                    |
| `Amount`           | Positive or negative value                         |
| `Type`             | Debit/Credit indicator (optional)                  |
| `External Account` | IBAN or payee name (used for expense accounts)     |
| `Currency`         | Optional (default is base currency)                |

Advanced options:
- Set **decimal separator** (`.` or `,`)
- Choose **date format**
- Reverse sign based on type/direction column

## 5. Duplicate Detection

To prevent importing duplicates:

- Each transaction is checked for:
  - Matching amount
  - Matching date (±1 day)
  - Matching description
  - Same source account

User can choose behavior on duplicates:
- Skip duplicates
- Import anyway with warning

Duplicates and errors are logged for review.

## 6. Import Tracking & History

Each import is tracked and visible to the user.

| Field         | Description                                |
|---------------|--------------------------------------------|
| Filename      | Original file name                         |
| StartedAt     | When the import started                    |
| FinishedAt    | When the import completed                  |
| TotalRows     | Number of records in the file              |
| SuccessCount  | Successfully imported transactions         |
| ErrorCount    | Rows skipped or failed                     |
| Status        | Pending / In Progress / Completed / Failed |

Users can:
- View all past imports
- See summary and details
- Export failed rows for correction

## 7. Background Processing with Hangfire

Large files (e.g. 10,000+ rows) are processed using **Hangfire background jobs**.

### 7.1 Processing Flow

1. After confirmation, the file and mapping are stored.
2. A `CsvImportJob` is enqueued via Hangfire.
3. The job:
   - Parses the file in batches (e.g. 250 rows per batch)
   - Validates and maps each row
   - Attempt to assign a category:
      - Based on destination account match
      - Based on historical rules or past descriptions (planned)
      - If no match: apply user's default category, if configured
   - Saves valid transactions
   - Logs duplicates or errors
4. Progress is updated in the `ImportJob` record.
5. Upon completion, the user receives a notification (toast, email, or UI update).

### 7.2 Hangfire Features Used

- Automatic retries for failed batches
- Job chaining for multi-step import processing
- Built-in dashboard for admin/job monitoring
- Stored state for job status, progress, and error logs


## 8. User Experience

- Users see an initial preview of sample data before importing
- Once submitted, the import runs in the background
- Progress indicator is shown (e.g. “4,500 / 10,000 rows processed”)
- Users can continue using the application without waiting
- A final notification is shown when the import is completed


## 9. Developer Notes

### Data Flow

```text
[CSV Upload] → [Preview + Mapping] → [Mapping Saved] → [Hangfire Job Queued] → [Batch Processing] → [ImportJob Record Updated] → [Notification Sent]
```


