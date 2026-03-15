using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures how Department entity maps to database table
    /// </summary>
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Table name
            builder.ToTable("Departments");

            // Primary Key
            builder.HasKey(d => d.DepartmentId);

            // Properties
            builder.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Location)
                .HasMaxLength(100);

            builder.Property(d => d.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(d => d.DepartmentName)
                .IsUnique()
                .HasDatabaseName("IX_Department_DepartmentName");

            builder.HasIndex(d => d.IsActive)
                .HasDatabaseName("IX_Department_IsActive");
        }
    }
}
