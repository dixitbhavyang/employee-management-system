using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for User operations
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int userId);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<int> SaveChangesAsync();
    }
}
