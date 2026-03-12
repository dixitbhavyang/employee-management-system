using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Entities
{
    /// <summary>
    /// Represents a user who can login to the system
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username for login (required, unique, max 50 chars)
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password (NEVER store plain text!)
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// User's email address (required, unique)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// User role (Admin or User)
        /// </summary>
        public UserRole Role { get; set; } = UserRole.User;

        /// <summary>
        /// Last time user logged in
        /// </summary>
        public DateTime? LastLoginDate { get; set; }
    }
}
