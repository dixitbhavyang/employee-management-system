using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for User database operations
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Add new user
        /// </summary>
        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        /// <summary>
        /// Update existing user
        /// </summary>
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return user;
        }

        /// <summary>
        /// Check if username exists
        /// </summary>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username && u.IsActive);
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email)
        {
            email = email.ToLower();
            return await _context.Users
                .AnyAsync(u => u.Email == email && u.IsActive);
        }

        /// <summary>
        /// Save changes to database
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
