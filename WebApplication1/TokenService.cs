using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1
{
    /// <summary>
    /// Class for tokens
    /// </summary>
    public class TokenService
    {
        /// <summary>
        /// The application configuration
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// Sets up the <see cref="_config"/>
        /// </summary>
        /// <param name="config">The configuration.</param>
        public TokenService(IConfiguration config)
        {
            this._config = config;
        }

        /// <summary>
        /// Creates a token
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="roles">Their roles</param>
        /// <returns>A JWT token based of the <paramref name="user"/> and their <paramref name="roles"/></returns>
        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            // Add the claims
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Add roles as multiple claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

#warning Secure this
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

            // Create and return token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}