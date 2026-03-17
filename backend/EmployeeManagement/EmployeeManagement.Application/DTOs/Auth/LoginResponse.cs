using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Auth
{
    /// <summary>
    /// Login response with JWT token
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
