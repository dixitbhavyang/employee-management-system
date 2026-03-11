-- =============================================
-- Employee Management System - Database Schema
-- Week 5: Full-Stack Project
-- =============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeManagementDB')
BEGIN
    ALTER DATABASE EmployeeManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EmployeeManagementDB;
END
GO

-- Create database
CREATE DATABASE EmployeeManagementDB;
GO

USE EmployeeManagementDB;
GO

-- =============================================
-- 1. DEPARTMENTS TABLE
-- =============================================
CREATE TABLE Departments
(
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    Location NVARCHAR(100),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NULL
);
GO

-- Index for common queries
CREATE INDEX IX_Departments_IsActive ON Departments(IsActive);
GO

-- =============================================
-- 2. EMPLOYEES TABLE
-- =============================================
CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(15),
    DepartmentId INT NOT NULL,
    JobTitle NVARCHAR(100),
    Salary DECIMAL(10,2) NOT NULL,
    HireDate DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NULL,
    
    CONSTRAINT FK_Employee_Department 
        FOREIGN KEY (DepartmentId) 
        REFERENCES Departments(DepartmentId),
    
    CONSTRAINT CHK_Employee_Salary CHECK (Salary >= 0),
    CONSTRAINT CHK_Employee_Email CHECK (Email LIKE '%@%')
);
GO

-- Indexes for performance
CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
CREATE INDEX IX_Employees_Email ON Employees(Email);
CREATE INDEX IX_Employees_IsActive ON Employees(IsActive);
CREATE INDEX IX_Employees_Name ON Employees(LastName, FirstName);
GO

-- =============================================
-- 3. USERS TABLE (for Authentication)
-- =============================================
CREATE TABLE Users
(
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'User', -- Admin, User
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    LastLoginDate DATETIME2 NULL,
    
    CONSTRAINT CHK_User_Role CHECK (Role IN ('Admin', 'User'))
);
GO

CREATE INDEX IX_Users_Username ON Users(Username);
GO

-- =============================================
-- 4. AUDIT LOG TABLE (Track changes)
-- =============================================
CREATE TABLE AuditLog
(
    AuditId INT PRIMARY KEY IDENTITY(1,1),
    TableName NVARCHAR(50) NOT NULL,
    RecordId INT NOT NULL,
    Action NVARCHAR(10) NOT NULL, -- INSERT, UPDATE, DELETE
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    ChangedBy NVARCHAR(50),
    ChangedDate DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- Get All Employees with Department Info
CREATE PROCEDURE sp_GetAllEmployees
    @IsActive BIT = NULL,
    @DepartmentId INT = NULL,
    @SearchTerm NVARCHAR(100) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    SELECT 
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.Email,
        e.Phone,
        e.DepartmentId,
        d.DepartmentName,
        e.JobTitle,
        e.Salary,
        e.HireDate,
        e.IsActive,
        e.CreatedDate,
        e.ModifiedDate,
        -- Total count for pagination
        COUNT(*) OVER() AS TotalRecords
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
    WHERE 
        (@IsActive IS NULL OR e.IsActive = @IsActive)
        AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
        AND (
            @SearchTerm IS NULL 
            OR e.FirstName LIKE '%' + @SearchTerm + '%'
            OR e.LastName LIKE '%' + @SearchTerm + '%'
            OR e.Email LIKE '%' + @SearchTerm + '%'
        )
    ORDER BY e.EmployeeId DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Get Employee by ID
CREATE PROCEDURE sp_GetEmployeeById
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.Email,
        e.Phone,
        e.DepartmentId,
        d.DepartmentName,
        e.JobTitle,
        e.Salary,
        e.HireDate,
        e.IsActive,
        e.CreatedDate,
        e.ModifiedDate
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
    WHERE e.EmployeeId = @EmployeeId;
END
GO

-- Create Employee
CREATE PROCEDURE sp_CreateEmployee
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(15),
    @DepartmentId INT,
    @JobTitle NVARCHAR(100),
    @Salary DECIMAL(10,2),
    @HireDate DATE,
    @CreatedBy NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Check if email already exists
        IF EXISTS (SELECT 1 FROM Employees WHERE Email = @Email)
        BEGIN
            RAISERROR('Email already exists', 16, 1);
            RETURN;
        END
        
        -- Check if department exists
        IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentId = @DepartmentId AND IsActive = 1)
        BEGIN
            RAISERROR('Invalid or inactive department', 16, 1);
            RETURN;
        END
        
        -- Insert employee
        INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId, JobTitle, Salary, HireDate)
        VALUES (@FirstName, @LastName, @Email, @Phone, @DepartmentId, @JobTitle, @Salary, @HireDate);
        
        DECLARE @NewEmployeeId INT = SCOPE_IDENTITY();
        
        -- Log audit
        INSERT INTO AuditLog (TableName, RecordId, Action, NewValue, ChangedBy)
        VALUES ('Employees', @NewEmployeeId, 'INSERT', 
                CONCAT('Created: ', @FirstName, ' ', @LastName), @CreatedBy);
        
        COMMIT TRANSACTION;
        
        -- Return new employee
        EXEC sp_GetEmployeeById @NewEmployeeId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Update Employee
CREATE PROCEDURE sp_UpdateEmployee
    @EmployeeId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(15),
    @DepartmentId INT,
    @JobTitle NVARCHAR(100),
    @Salary DECIMAL(10,2),
    @HireDate DATE,
    @IsActive BIT,
    @ModifiedBy NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Check if employee exists
        IF NOT EXISTS (SELECT 1 FROM Employees WHERE EmployeeId = @EmployeeId)
        BEGIN
            RAISERROR('Employee not found', 16, 1);
            RETURN;
        END
        
        -- Check if email is taken by another employee
        IF EXISTS (SELECT 1 FROM Employees WHERE Email = @Email AND EmployeeId != @EmployeeId)
        BEGIN
            RAISERROR('Email already exists', 16, 1);
            RETURN;
        END
        
        -- Check if department exists
        IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentId = @DepartmentId AND IsActive = 1)
        BEGIN
            RAISERROR('Invalid or inactive department', 16, 1);
            RETURN;
        END
        
        -- Update employee
        UPDATE Employees
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            Phone = @Phone,
            DepartmentId = @DepartmentId,
            JobTitle = @JobTitle,
            Salary = @Salary,
            HireDate = @HireDate,
            IsActive = @IsActive,
            ModifiedDate = GETDATE()
        WHERE EmployeeId = @EmployeeId;
        
        -- Log audit
        INSERT INTO AuditLog (TableName, RecordId, Action, NewValue, ChangedBy)
        VALUES ('Employees', @EmployeeId, 'UPDATE', 
                CONCAT('Updated: ', @FirstName, ' ', @LastName), @ModifiedBy);
        
        COMMIT TRANSACTION;
        
        -- Return updated employee
        EXEC sp_GetEmployeeById @EmployeeId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Delete Employee (Soft Delete)
CREATE PROCEDURE sp_DeleteEmployee
    @EmployeeId INT,
    @DeletedBy NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Employees WHERE EmployeeId = @EmployeeId)
        BEGIN
            RAISERROR('Employee not found', 16, 1);
            RETURN;
        END
        
        -- Soft delete
        UPDATE Employees
        SET IsActive = 0, ModifiedDate = GETDATE()
        WHERE EmployeeId = @EmployeeId;
        
        -- Log audit
        INSERT INTO AuditLog (TableName, RecordId, Action, NewValue, ChangedBy)
        VALUES ('Employees', @EmployeeId, 'DELETE', 'Soft deleted', @DeletedBy);
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Get All Departments
CREATE PROCEDURE sp_GetAllDepartments
    @IsActive BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        DepartmentId,
        DepartmentName,
        Location,
        IsActive,
        CreatedDate,
        ModifiedDate,
        -- Count employees
        (SELECT COUNT(*) FROM Employees WHERE DepartmentId = d.DepartmentId AND IsActive = 1) AS EmployeeCount
    FROM Departments d
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY DepartmentName;
END
GO

-- User Authentication
CREATE PROCEDURE sp_AuthenticateUser
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @UserId INT;
    
    SELECT 
        @UserId = UserId
    FROM Users
    WHERE Username = @Username 
      AND PasswordHash = @PasswordHash 
      AND IsActive = 1;
    
    IF @UserId IS NOT NULL
    BEGIN
        -- Update last login
        UPDATE Users
        SET LastLoginDate = GETDATE()
        WHERE UserId = @UserId;
        
        -- Return user details
        SELECT 
            UserId,
            Username,
            Email,
            FullName,
            Role,
            IsActive
        FROM Users
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        RAISERROR('Invalid username or password', 16, 1);
    END
END
GO

-- =============================================
-- SAMPLE DATA
-- =============================================

-- Insert Departments
INSERT INTO Departments (DepartmentName, Location) VALUES
('Information Technology', 'Bangalore'),
('Human Resources', 'Mumbai'),
('Finance', 'Delhi'),
('Marketing', 'Pune'),
('Sales', 'Hyderabad');
GO

-- Insert Sample Employees
INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId, JobTitle, Salary, HireDate) VALUES
('Raj', 'Kumar', 'raj.kumar@company.com', '9876543210', 1, 'Senior Developer', 75000, '2022-01-15'),
('Priya', 'Sharma', 'priya.sharma@company.com', '9876543211', 1, 'Developer', 60000, '2022-03-20'),
('Amit', 'Patel', 'amit.patel@company.com', '9876543212', 2, 'HR Manager', 65000, '2021-06-10'),
('Neha', 'Singh', 'neha.singh@company.com', '9876543213', 3, 'Accountant', 55000, '2023-01-05'),
('Vikram', 'Reddy', 'vikram.reddy@company.com', '9876543214', 1, 'Tech Lead', 90000, '2020-11-12'),
('Anita', 'Desai', 'anita.desai@company.com', '9876543215', 4, 'Marketing Manager', 70000, '2022-07-18'),
('Suresh', 'Nair', 'suresh.nair@company.com', '9876543216', 5, 'Sales Executive', 50000, '2023-02-28'),
('Kavita', 'Mehta', 'kavita.mehta@company.com', '9876543217', 1, 'Developer', 58000, '2023-04-10'),
('Rahul', 'Gupta', 'rahul.gupta@company.com', '9876543218', 3, 'Finance Analyst', 62000, '2022-09-15'),
('Sanjay', 'Verma', 'sanjay.verma@company.com', '9876543219', 2, 'HR Executive', 48000, '2023-05-22');
GO

-- Insert Sample User (Password: "Admin@123" - you should hash this properly in production!)
INSERT INTO Users (Username, PasswordHash, Email, FullName, Role) VALUES
('admin', 'Admin@123', 'admin@company.com', 'System Administrator', 'Admin'),
('user', 'User@123', 'user@company.com', 'Regular User', 'User');
GO

-- =============================================
-- VIEWS
-- =============================================

-- Employee Summary View
CREATE VIEW vw_EmployeeSummary
AS
SELECT 
    e.EmployeeId,
    CONCAT(e.FirstName, ' ', e.LastName) AS FullName,
    e.Email,
    e.Phone,
    d.DepartmentName,
    e.JobTitle,
    e.Salary,
    e.HireDate,
    DATEDIFF(YEAR, e.HireDate, GETDATE()) AS YearsOfService,
    e.IsActive
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId;
GO

-- Department Statistics View
CREATE VIEW vw_DepartmentStatistics
AS
SELECT 
    d.DepartmentId,
    d.DepartmentName,
    d.Location,
    COUNT(e.EmployeeId) AS TotalEmployees,
    AVG(e.Salary) AS AverageSalary,
    MIN(e.Salary) AS MinSalary,
    MAX(e.Salary) AS MaxSalary,
    SUM(e.Salary) AS TotalSalaryExpense
FROM Departments d
LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId AND e.IsActive = 1
WHERE d.IsActive = 1
GROUP BY d.DepartmentId, d.DepartmentName, d.Location;
GO

-- =============================================
-- VERIFICATION QUERIES
-- =============================================

PRINT 'Database setup completed successfully!';
PRINT '';
PRINT 'Tables created:';
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
PRINT '';
PRINT 'Sample data inserted:';
SELECT 'Departments' AS TableName, COUNT(*) AS RecordCount FROM Departments
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'Users', COUNT(*) FROM Users;
GO

-- Test stored procedures
PRINT '';
PRINT 'Testing stored procedures:';
PRINT 'Getting all employees...';
EXEC sp_GetAllEmployees @PageNumber = 1, @PageSize = 5;
PRINT '';
PRINT 'Getting all departments...';
EXEC sp_GetAllDepartments;
GO
