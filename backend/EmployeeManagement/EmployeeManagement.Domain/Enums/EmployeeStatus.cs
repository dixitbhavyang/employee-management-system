using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Enums
{
    /// <summary>
    /// Defines employee status in the system
    /// </summary>
    public enum EmployeeStatus
    {
        /// <summary>
        /// Employee is currently working
        /// </summary>
        Active = 0,

        /// <summary>
        /// Employee is on leave
        /// </summary>
        OnLeave = 1,

        /// <summary>
        /// Employee has resigned/terminated
        /// </summary>
        Inactive = 2
    }
}
