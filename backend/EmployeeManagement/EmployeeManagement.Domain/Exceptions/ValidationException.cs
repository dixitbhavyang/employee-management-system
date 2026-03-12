using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when business validation fails
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
