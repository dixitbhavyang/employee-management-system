using EmployeeManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Entities
{
    /// <summary>
    /// Represents an employee in the organization
    /// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Employee's first name (required, max 50 chars)
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Employee's last name (required, max 50 chars)
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Employee's email address (required, unique)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Employee's phone number (optional)
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Department ID (foreign key)
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Job title/position
        /// </summary>
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// Monthly salary
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Date when employee was hired
        /// </summary>
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        // ===== NAVIGATION PROPERTY =====
        /// <summary>
        /// Navigation property to Department
        /// </summary>
        public Department? Department { get; set; }
    }
}
