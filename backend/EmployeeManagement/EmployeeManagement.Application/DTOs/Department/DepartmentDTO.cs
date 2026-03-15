using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Department
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int EmployeeCount { get; set; }  // Computed - how many employees
    }
}
