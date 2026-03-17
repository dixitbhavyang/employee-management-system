using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Register new user
        /// </summary>
        Task<LoginResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Verify if username exists
        /// </summary>
        Task<bool> UsernameExistsAsync(string username);
    }
}
