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
- ✅ Person CRUD (clients management)
- ✅ Loan CRUD with automatic installment generation
- ✅ View loan installments with payment status tracking
- ✅ Register payments against installments
- ✅ Search, pagination, and sorting across all entities
- ✅ Real-time status updates (Pending, Partial, Paid)

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
- `GET /api/persons` - Get paginated persons (supports search, sort)
- `GET /api/persons/{id}` - Get person by ID
- `POST /api/persons` - Create person
- `PUT /api/persons/{id}` - Update person
- `DELETE /api/persons/{id}` - Delete person

### Loans
- `GET /api/loans` - Get paginated loans (supports search, sort)
- `GET /api/loans/{id}` - Get loan by ID
- `GET /api/loans/person/{personId}` - Get loans by person
- `POST /api/loans` - Create loan (auto-generates installments)
- `PUT /api/loans/{id}` - Update loan
- `DELETE /api/loans/{id}` - Delete loan (cascades to installments/payments)

### Installments
- `GET /api/installments/loan/{loanId}` - Get all installments for a loan
- `GET /api/installments/pending/loan/{loanId}` - Get pending installments
- `GET /api/installments/{id}` - Get installment by ID

### Payments
- `GET /api/payments/loan/{loanId}` - Get all payments for a loan
- `GET /api/payments/installment/{installmentId}` - Get payments for installment
- `POST /api/payments` - Register a payment

## Business Rules
- Loans automatically generate monthly installments on creation
- Interest is calculated and added to monthly payment amount
- Payment day must be between 1-28 of the month
- Installment status: `Pending` (0), `Partial` (1), `Paid` (2)
- When editing a loan, amount and months are locked (only interest rate and payment day editable)
- Deleting a loan cascades to all installments and payments

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
