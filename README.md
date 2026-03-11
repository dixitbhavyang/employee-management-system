# Employee Management System

A full-stack web app to manage employees and departments.

**Built with:** ASP.NET Core 8 · Angular 17 · SQL Server · Entity Framework Core

---

## What it does

- Add, edit, delete employees and departments
- Login with JWT authentication
- Search, filter, and paginate employee data
- Admin and User roles
- Dashboard with basic stats

---

## Tech Used

**Backend:** ASP.NET Core 8, Clean Architecture, EF Core, SQL Server, JWT, Swagger

**Frontend:** Angular 17, Reactive Forms, RxJS, Bootstrap 5

---

## Project Structure

```
EmployeeManagement/
├── backend/
│   ├── Domain/          # Entities
│   ├── Application/     # Services, DTOs
│   ├── Infrastructure/  # DB, Repositories
│   └── API/             # Controllers
│
├── frontend/
│   └── app/
│       ├── core/        # Auth, Guards
│       ├── shared/      # Reusable components
│       └── features/    # Employees, Departments, Dashboard
│
└── database/
    └── EmployeeManagementDB.sql
```

---

## How to Run

**Backend**
```bash
cd backend
dotnet restore
dotnet ef database update --project src/EmployeeManagement.Infrastructure --startup-project src/EmployeeManagement.API
dotnet run --project src/EmployeeManagement.API
# Runs on https://localhost:5001
```

**Frontend**
```bash
cd frontend
npm install
ng serve
# Runs on http://localhost:4200
```

**Default Login**
```
admin / Admin@123
user  / User@123
```

---

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | /api/auth/login | Login |
| GET | /api/employees | Get all employees |
| POST | /api/employees | Add employee |
| PUT | /api/employees/{id} | Update employee |
| DELETE | /api/employees/{id} | Delete employee |
| GET | /api/departments | Get all departments |

Swagger docs: `https://localhost:5001/swagger`

---

**Author:** Bhavyang Dixit · [GitHub](https://github.com/dixitbhavyang)
