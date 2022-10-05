namespace WebApplication1
{
    /// <summary>
    /// A DTO for the <see cref="ApplicationUser"/> during login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The email the <see cref="ApplicationUser"/> is trying to login with
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password the <see cref="ApplicationUser"/> is trying to login with
        /// </summary>
        public string Password { get; set; }
    }
}