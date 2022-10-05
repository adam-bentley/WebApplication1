namespace WebApplication1
{
    /// <summary>
    /// A DTO for the <see cref="ApplicationUser"/>.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The username of the <see cref="ApplicationUser"/>.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The firstname of the <see cref="ApplicationUser"/>.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The surname of the <see cref="ApplicationUser"/>.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The token of the <see cref="ApplicationUser"/>.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The signature of the <see cref="ApplicationUser"/>.
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// A <see cref="IList{T}"/> of roles the <see cref="ApplicationUser"/> has.
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}