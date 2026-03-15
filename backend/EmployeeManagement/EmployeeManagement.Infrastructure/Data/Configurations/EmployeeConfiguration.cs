using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures how Employee entity maps to database table
    /// </summary>
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Table name
            builder.ToTable("Employees");

            // Primary Key
            builder.HasKey(e => e.EmployeeId);

            // Properties configuration
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Phone)
                .HasMaxLength(15);

            builder.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(e => e.HireDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("IX_Employee_Email");

            builder.HasIndex(e => e.DepartmentId)
                .HasDatabaseName("IX_Employee_DepartmentId");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_Employee_IsActive");

            // Relationships
            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting department with employees
        }
    }
}
