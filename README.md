# 🏢 Employee Management System

A professional full-stack web application for managing employees, departments, and organizational data. Built with modern technologies and enterprise architecture patterns.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-17-DD0031?logo=angular)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [API Documentation](#-api-documentation)
- [Screenshots](#-screenshots)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

### Core Features
- ✅ **Employee Management** - Complete CRUD operations
- ✅ **Department Management** - Organize employees by departments
- ✅ **User Authentication** - Secure JWT-based authentication
- ✅ **Search & Filter** - Advanced search and filtering capabilities
- ✅ **Pagination** - Efficient data loading with server-side pagination
- ✅ **Form Validation** - Client and server-side validation
- ✅ **Responsive Design** - Mobile-friendly UI

### Advanced Features
- 📊 **Dashboard** - Statistics and insights
- 📈 **Reports** - Employee and department reports
- 🔐 **Role-Based Access** - Admin and User roles
- 📝 **Audit Logging** - Track all data changes
- 🔍 **Advanced Search** - Multi-criteria search
- 📄 **Export** - Export data to Excel/PDF

---

## 🛠️ Tech Stack

### Backend
- **Framework:** ASP.NET Core 8.0 Web API
- **Architecture:** Clean Architecture (4-Layer)
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server 2022
- **Authentication:** JWT Bearer Tokens
- **API Documentation:** Swagger/OpenAPI
- **Logging:** Serilog
- **Validation:** FluentValidation

### Frontend
- **Framework:** Angular 17
- **UI Components:** Custom components + Bootstrap 5
- **State Management:** RxJS Observables
- **Forms:** Reactive Forms
- **HTTP Client:** Angular HttpClient
- **Routing:** Angular Router with Guards
- **Build Tool:** Angular CLI

### DevOps
- **Version Control:** Git & GitHub
- **CI/CD:** GitHub Actions
- **Containerization:** Docker (optional)
- **Hosting:** Azure App Service / IIS

---

## 🏗️ Architecture

### Backend - Clean Architecture

```
┌─────────────────────────────────────────┐
│           API Layer                     │  ← Controllers, Middleware
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│      Infrastructure Layer               │  ← DbContext, Repositories
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│       Application Layer                 │  ← Services, DTOs, Interfaces
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│         Domain Layer                    │  ← Entities, Business Rules
└─────────────────────────────────────────┘
```

**Benefits:**
- ✅ Testable
- ✅ Maintainable
- ✅ Scalable
- ✅ Independent of frameworks

### Frontend - Feature Module Architecture

```
App Module
  │
  ├── Core Module (Singleton Services)
  │   └── Auth, Interceptors, Guards
  │
  ├── Shared Module (Reusable Components)
  │   └── Pagination, Table, Buttons
  │
  └── Feature Modules (Lazy Loaded)
      ├── Employees Module
      ├── Departments Module
      └── Dashboard Module
```

---

## 📁 Project Structure

```
EmployeeManagement/
│
├── backend/
│   ├── src/
│   │   ├── EmployeeManagement.Domain/         # Entities, Enums
│   │   ├── EmployeeManagement.Application/    # Services, DTOs
│   │   ├── EmployeeManagement.Infrastructure/ # DbContext, Repos
│   │   └── EmployeeManagement.API/            # Controllers
│   │
│   └── tests/
│       ├── EmployeeManagement.UnitTests/
│       └── EmployeeManagement.IntegrationTests/
│
├── frontend/
│   └── src/
│       └── app/
│           ├── core/         # Auth, Guards, Interceptors
│           ├── shared/       # Reusable Components
│           ├── features/     # Feature Modules
│           └── layout/       # App Layout
│
├── database/
│   └── EmployeeManagementDB.sql
│
├── docs/
│   ├── API_Documentation.md
│   └── Architecture.md
│
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+ & npm](https://nodejs.org/)
- [SQL Server 2019+](https://www.microsoft.com/sql-server)
- [Angular CLI](https://angular.io/cli): `npm install -g @angular/cli`
- [Git](https://git-scm.com/)

### Installation

#### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/employee-management-system.git
cd employee-management-system
```

#### 2. Setup Database

```bash
# Open SQL Server Management Studio
# Run the script: database/EmployeeManagementDB.sql
```

#### 3. Setup Backend

```bash
cd backend

# Restore packages
dotnet restore

# Update connection string in appsettings.json
# Update: "DefaultConnection" in src/EmployeeManagement.API/appsettings.json

# Run migrations (if using EF migrations)
dotnet ef database update --project src/EmployeeManagement.Infrastructure --startup-project src/EmployeeManagement.API

# Run the API
cd src/EmployeeManagement.API
dotnet run
```

Backend will run on: `https://localhost:5001`

#### 4. Setup Frontend

```bash
cd frontend

# Install dependencies
npm install

# Update API URL in environment file
# Update: apiUrl in src/environments/environment.ts

# Run the application
ng serve
```

Frontend will run on: `http://localhost:4200`

#### 5. Login Credentials

```
Username: admin
Password: Admin@123

Username: user
Password: User@123
```

---

## 📚 API Documentation

Once the backend is running, access Swagger UI at:
```
https://localhost:5001/swagger
```

### Key Endpoints

#### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

#### Employees
- `GET /api/employees` - Get all employees (with pagination)
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

#### Departments
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create new department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department

---

## 📸 Screenshots

### Dashboard
![Dashboard](docs/screenshots/dashboard.png)

### Employee List
![Employee List](docs/screenshots/employee-list.png)

### Employee Form
![Employee Form](docs/screenshots/employee-form.png)

---

## 🧪 Running Tests

### Backend Tests

```bash
cd backend

# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/EmployeeManagement.UnitTests

# Run integration tests
dotnet test tests/EmployeeManagement.IntegrationTests

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Frontend Tests

```bash
cd frontend

# Run unit tests
ng test

# Run e2e tests
ng e2e

# Run tests with coverage
ng test --code-coverage
```

---

## 🐳 Docker (Optional)

```bash
# Build and run with Docker Compose
docker-compose up -d

# Stop containers
docker-compose down
```

---

## 📖 Development

### Backend Development

```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/EmployeeManagement.Infrastructure --startup-project src/EmployeeManagement.API

# Update database
dotnet ef database update --project src/EmployeeManagement.Infrastructure --startup-project src/EmployeeManagement.API

# Watch for changes (hot reload)
dotnet watch run --project src/EmployeeManagement.API
```

### Frontend Development

```bash
# Generate new component
ng generate component features/employees/pages/employee-list

# Generate new service
ng generate service features/employees/services/employee

# Generate new module
ng generate module features/reports --routing

# Build for production
ng build --configuration production
```

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Your Name**
- GitHub: [@dixitbhavyang](https://github.com/YOUR_USERNAME)
- LinkedIn: [Your LinkedIn](https://linkedin.com/in/YOUR_PROFILE)
- Email: your.email@example.com

---

## 🙏 Acknowledgments

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Angular Documentation](https://angular.io/docs)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- Clean Architecture by Robert C. Martin

---

## 📊 Project Status

🚀 **Active Development** - This project is actively maintained and updated regularly.

### Roadmap
- [x] Basic CRUD operations
- [x] Authentication & Authorization
- [x] Search & Filter
- [x] Pagination
- [ ] Dashboard with charts
- [ ] Export to Excel/PDF
- [ ] Email notifications
- [ ] File upload (profile pictures)
- [ ] Advanced reporting
- [ ] Multi-language support

---

**⭐ If you find this project useful, please consider giving it a star!**
