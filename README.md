# LolaCredits

Loan management system with installments and payment tracking — built with .NET 8 backend and Vue 3 frontend.

## Overview
- **Backend:** `backLolaCredits/` (.NET 8 Web API + EF Core + SQLite)
- **Frontend:** `frontLolaCredits/` (Vue 3 + Vite + Tailwind CSS)

## Tech Stack
**Backend:**
- .NET 8, C# 12, ASP.NET Core Web API
- Entity Framework Core 8 with SQLite
- AutoMapper for DTO mapping

**Frontend:**
- Vue 3 (Composition API with `<script setup>`)
- Vite 6
- Tailwind CSS 3
- Axios for HTTP requests

## Features
- ✅ Person CRUD (clients management) with pagination, search, and sorting
- ✅ Loan CRUD with automatic installment generation
- ✅ Email and CI columns visible in loans table for quick reference
- ✅ View loan installments with payment status tracking
- ✅ Register payments against installments with validation
- ✅ Sequential payment validation (can't pay future installments before previous ones)
- ✅ Dashboard with comprehensive statistics and quick actions
- ✅ Real-time status updates (Pending, Partial, Paid)
- ✅ Complete Spanish UI
- ✅ Modal blur effects for better UX

## Architecture
**Backend flow:**
```
Controllers → Services → EF Core DbContext → SQLite
```

**Data model:**
```
Person → Loan → Installment → Payment
```

**Frontend pattern:**
```
Views → Components (Table, Modal) → Services (Axios) → API
```

## Quick Start

### Backend Setup
1. Navigate to backend folder:
```bash
cd backLolaCredits
```

2. Restore dependencies and build:
```bash
dotnet restore
dotnet build
```

3. Run the API:
```bash
dotnet run --urls "http://localhost:5228"
```

The API will be available at `http://localhost:5228`.

### Frontend Setup
1. Navigate to frontend folder:
```bash
cd frontLolaCredits
```

2. Install dependencies:
```bash
npm install
```

3. Create `.env` file with:
```
VITE_API_BASE=http://localhost:5228
```

4. Run development server:
```bash
npm run dev
```

The app will be available at `http://localhost:5173`.

## API Endpoints

### Persons
- `GET /api/persons` - Get paginated persons (supports search by name/CI/email, sort)
- `GET /api/persons/{id}` - Get person by ID
- `GET /api/persons/all` - Get all persons (for dashboard stats)
- `POST /api/persons` - Create person
- `PUT /api/persons/{id}` - Update person
- `DELETE /api/persons/{id}` - Delete person

### Loans
- `GET /api/loans` - Get paginated loans (supports search by client/CI/email, sort)
- `GET /api/loans/{id}` - Get loan by ID with installments
- `POST /api/loans` - Create loan (auto-generates installments)
- `PUT /api/loans/{id}` - Update loan (only interest rate and payment day)
- `DELETE /api/loans/{id}` - Delete loan (cascades to installments/payments)

### Installments
- `GET /api/installments/loan/{loanId}` - Get all installments for a loan

### Payments
- `POST /api/payments` - Register a payment (validates sequential payment rule)

### Dashboard
- `GET /api/dashboard/stats` - Get dashboard statistics (total clients, loans, amounts, etc.)

## Business Rules
- Loans automatically generate monthly installments on creation
- Interest is calculated and added to monthly payment amount
- Payment day must be between 1-28 of the month
- Installment status: `Pending` (0), `Partial` (1), `Paid` (2)
- **Sequential payment validation:** Cannot pay an installment if previous installments are not fully paid
- When editing a loan, amount and months are locked (only interest rate and payment day editable)
- Deleting a loan cascades to all installments and payments
- All monetary values rounded to 2 decimals

## Database
SQLite database is created automatically on first run. Migrations are in `backLolaCredits/Migrations/`.

To reset database:
```bash
rm LolaCredits.db
dotnet ef database update
```

## Development Notes
- Backend uses PascalCase (default .NET serialization)
- Frontend expects camelCase (axios configured accordingly)
- All monetary values rounded to 2 decimals
- Debounced search (500ms) to reduce API calls
- Pagination defaults to 10 items per page
