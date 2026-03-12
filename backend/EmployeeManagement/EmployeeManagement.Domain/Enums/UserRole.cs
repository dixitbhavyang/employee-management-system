using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Enums
{
    /// <summary>
    /// Defines user roles in the system
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Regular user - read-only access
        /// </summary>
        User = 0,

        /// <summary>
        /// Administrator - full access
        /// </summary>
        Admin = 1
    }
}
