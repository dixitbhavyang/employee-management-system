namespace EmployeeManagement.Api.Configuration
{
    /// <summary>
    /// JWT configuration settings
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Secret key for signing JWT tokens
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer (who created the token)
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience (who can use the token)
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration in minutes
        /// </summary>
        public int ExpirationInMinutes { get; set; } = 60;
    }
}
