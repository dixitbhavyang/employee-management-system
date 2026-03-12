using EmployeeManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Entities
{
    /// <summary>
    /// Represents a department in the organization
    /// </summary>
    public class Department : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the department
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Department name (required, unique, max 100 chars)
        /// </summary>
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>
        /// Department location/office (optional)
        /// </summary>
        public string? Location { get; set; }

        // ===== NAVIGATION PROPERTY =====
        /// <summary>
        /// Collection of employees in this department
        /// </summary>
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
