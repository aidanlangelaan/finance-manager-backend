# Project Plan - Personal Finance Manager

## 1. Overview

This project aims to build a modern, self-hosted personal finance management application inspired by tools like Firefly III. The application will allow users to track their finances, categorize spending, and analyze financial trends. It will be built with modularity, privacy, and usability in mind.

## 2. Objectives

- Provide a user-friendly web interface to manage transactions and accounts.
- Allow CSV import/export from banks.
- Support multi-currency and budgeting features.
- Provide secure user authentication (Keycloak or similar).
- Offer detailed reporting and visualizations.

## 3. Technology Stack

### Frontend
- **Framework**: Angular
- **UI Library**: Angular Material
- **State Management**: Signals + built-in Angular tools

### Backend
- **Language**: .NET (C#)
- **Architecture**: Clean Architecture (layered approach)
- **API Design**: RESTful endpoints
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: Keycloak integration using OpenID Connect

### DevOps
- **Containerization**: Docker & Docker Compose
- **Reverse Proxy**: Nginx
- **Hosting**: Self-hosted VPS with SSL (Let's Encrypt)

## 4. Modules & Features

### 4.1 User Management
- Login, registration, password reset
- Role-based access (if needed)
- Integration with Keycloak

### 4.2 Accounts
- Bank accounts with balance tracking
- Account types (cash, credit, savings)

### 4.3 Transactions
- Manual entry and CSV import
- Categorization (tags, categories)
- Recurring transactions

### 4.4 Budgets
- Monthly/weekly budgets
- Budget vs actuals reporting

### 4.5 Reports & Dashboards
- Expense vs income over time
- Category breakdowns
- Net worth tracker

### 4.6 Settings
- Currency preferences
- Language & theme support
- Import/export data

## 5. Development Phases

### Phase 1 - Backend Foundation
- Project scaffolding with Clean Architecture structure
- PostgreSQL setup via Docker
- Keycloak integration and securing API endpoints
- Define and implement core domain models: UserProfile, Account, Transaction
- Set up EF Core migrations and initial database seeding
- Implement basic REST API endpoints for authentication, accounts, and transactions
- Write unit and integration tests for services and repositories

### Phase 2 - Frontend Foundation
- Angular project scaffolding with modular structure (core, shared, features)
- Integrate authentication via Keycloak (OIDC)
- Set up basic layout using Angular Material (toolbar, sidenav, footer)
- Implement routing and authentication guards
- Add initial dashboard placeholder and theme switching

### Phase 3 - Core Features
#### Backend
- Implement CSV import logic
- Add recurring transaction support
- Implement budget domain model and logic

#### Frontend
- Implement CRUD interface for accounts and transactions
- Add CSV import/export functionality
- Display initial dashboard charts and overviews

### Phase 4 - Reporting & Settings
- Implement budget vs actuals overview
- Add reporting views (e.g. category breakdown, income vs expenses)
- Add settings module: currency preferences, language, theme support
- (Optional) Add PWA support for offline functionality

### Phase 5 - Polish & Deployment
- Improve error handling, form validation, and user feedback
- Polish UI and improve accessibility
- Create Docker Compose setup including frontend, backend, Keycloak, and PostgreSQL
- Configure Nginx as a reverse proxy with SSL (Let's Encrypt)
- Final deployment to self-hosted VPS

## 6. Stretch Goals

- Mobile-friendly PWA support
- Multi-user support
- AI-based transaction categorization

## 7. License & Privacy

This project is licensed under the [Polyform Noncommercial License 1.0.0](https://polyformproject.org/licenses/noncommercial/1.0.0/). 

You may use, modify, and share this code **for non-commercial purposes only**. 

If you're interested in commercial usage or partnerships, feel free to reach out.

No data will leave the user's instance. Privacy and self-control over data are core values of this project.

