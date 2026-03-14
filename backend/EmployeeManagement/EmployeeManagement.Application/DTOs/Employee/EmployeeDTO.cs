using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Employee
{
    /// <summary>
    /// Data Transfer Object for Employee (what API returns)
    /// </summary>
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        // Computed property - not in database!
        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public int DepartmentId { get; set; }

        // Just the department name, not the whole object
        public string DepartmentName { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        // Formatted for display
        public string SalaryFormatted => $"₹{Salary:N0}";

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
