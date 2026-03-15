using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Department
{
    public class CreateDepartmentRequest
    {
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Location { get; set; }
    }
}
