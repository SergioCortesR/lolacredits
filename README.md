# LolaCredits

Web App to manage loans through a project called **LolaCredits** using a back (asp.net 8) and a front end framework (vue 3 + vite)

## Contenido
- `backLolaCredits/` — Web API (.NET 8) con modelos, servicios, controladores y EF Core.
- `frontLolaCredits/` — interfaz en Vue 3 (Vite).

## Stack técnico
- Backend: .NET 8, C#, EF Core (SQLite)
- Frontend: Vue 3, Vite
- Mapeos: AutoMapper

## Arquitectura
- Controllers → Services → AppDbContext (EF Core)
- Entidades principales: `Person` → `Loan` → `Installment` → `Payment`

## Requisitos previos
- .NET 8 SDK instalado
- Node.js (para el frontend)
- `dotnet-ef` si vas a gestionar migraciones localmente: `dotnet tool install --global dotnet-ef`

## Instrucciones backend
1. Abrir una terminal en `backLolaCredits/`.
2. Restaurar dependencias y compilar:

```bash
dotnet restore
dotnet build
```

3. Si quieres recrear las migraciones/DB:

```bash
# Elimina la carpeta Migrations y la base de datos SQLite si quieres empezar limpio.
# Ajusta el nombre del archivo DB si es diferente.
# LolaCredits

Small app to manage loans, installments and payments — backend in .NET 8 (SQLite + EF Core) and a simple Vue 3 frontend.

## Quick overview
- Backend: `backLolaCredits/` (.NET 8 Web API)
- Frontend: `frontLolaCredits/` (Vue 3 + Vite)

## Tech stack
- .NET 8, EF Core (SQLite)
- Vue 3, Vite
- AutoMapper for DTO mapping

## Minimum setup (backend)
1. Open a terminal in `backLolaCredits/`.
2. Restore and build:

```bash
dotnet restore
dotnet build
```

3. Run the API:

```bash
dotnet run --urls "http://localhost:5228"
```

The API will run at `http://localhost:5228` by default.

## Setup frontend
1. Open a terminal in `frontLolaCredits/`.
2. Install and run:

```bash
npm install
npm run dev
```

## Notes
- Monetary values are rounded to 2 decimals in backend services.
- Installment status uses an enum (`Pending`, `Partial`, `Paid`).
- Payments cannot be made for an installment if previous installments are not `Paid`.