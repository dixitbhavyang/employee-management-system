using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
