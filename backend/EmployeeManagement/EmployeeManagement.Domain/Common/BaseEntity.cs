using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Common
{
    /// <summary>
    /// Base class for all entities - contains common properties
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// When was this record created?
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When was this record last updated?
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Is this record active? (for soft delete)
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
