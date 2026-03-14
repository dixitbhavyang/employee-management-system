using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Common
{
    /// <summary>
    /// Generic wrapper for paginated results
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>
        /// The actual data items for this page
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Total number of records in database (all pages)
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        /// <summary>
        /// Is there a previous page?
        /// </summary>
        public bool HasPrevious => PageNumber > 1;

        /// <summary>
        /// Is there a next page?
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;
    }
}
