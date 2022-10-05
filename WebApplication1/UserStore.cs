using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace WebApplication1
{
    public class UserStore : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly string _connectionString = "host = 127.0.0.1;port=5432;Username=postgres;password=password;database=spacestation";

        public UserStore()
        {
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            string sql = @"INSERT INTO users (firstname, lastname, email, normalized_email, email_confirmed,  normalized_username, username, password) VALUES
							(@firstname, @lastname, @email, @normalized_email, @email_confirmed, @normalized_username, @username, @password) RETURNING ID;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("firstname", user.FirstName);
                command.Parameters.AddWithValue("lastname", user.LastName);
                command.Parameters.AddWithValue("email", user.Email);
                command.Parameters.AddWithValue("normalized_email", user.NormalizedEmail);
                command.Parameters.AddWithValue("email_confirmed", user.EmailConfirmed);
                command.Parameters.AddWithValue("username", user.Email);
                command.Parameters.AddWithValue("normalized_username", user.NormalizedEmail);
                command.Parameters.AddWithValue("password", user.Password);
                user.Id = Convert.ToInt16(await command.ExecuteScalarAsync(cancellationToken));
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            string sql = @"DELETE FROM users WHERE id = @id";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("id", user.Id);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            string sql = @"SELECT id, firstname, lastname, email, normalized_email, email_confirmed, password, username, normalized_username FROM users WHERE normalized_email = @email;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("email", normalizedEmail);
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return new ApplicationUser()
                    {
                        Id = reader.GetInt16(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        NormalizedEmail = reader.GetString(4),
                        EmailConfirmed = reader.GetBoolean(5),
                        Password = reader.GetString(6),
                        Username = reader.GetString(7),
                        NormalizedUsername = reader.GetString(8),
                    };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            string sql = @"SELECT id, firstname, lastname, email, normalized_email, email_confirmed, password, username, normalized_username FROM users WHERE id = @id;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("id", userId);
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return new ApplicationUser()
                    {
                        Id = reader.GetInt16(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        NormalizedEmail = reader.GetString(4),
                        EmailConfirmed = reader.GetBoolean(5),
                        Password = reader.GetString(6),
                        Username = reader.GetString(7),
                        NormalizedUsername = reader.GetString(8),
                    };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            string sql = @"SELECT id, firstname, lastname, email, normalized_email, email_confirmed, password, username, normalized_username FROM users WHERE normalized_email = @normalized_email;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("normalized_email", normalizedUserName);
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return new ApplicationUser()
                    {
                        Id = reader.GetInt16(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        NormalizedEmail = reader.GetString(4),
                        EmailConfirmed = reader.GetBoolean(5),
                        Password = reader.GetString(6),
                        Username = reader.GetString(7),
                        NormalizedUsername = reader.GetString(8),
                    };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUsername);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password != null);
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            string sql = @"UPDATE users SET firstname=@firstname, lastname=@lastname, email=@email, normalized_email=@normalized_email, email_confirmed=@email_confirmed, password=@password WHERE ID=@id;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("firstname", user.FirstName);
                command.Parameters.AddWithValue("lastname", user.LastName);
                command.Parameters.AddWithValue("email", user.Email);
                command.Parameters.AddWithValue("normalized_email", user.NormalizedEmail);
                command.Parameters.AddWithValue("email_confirmed", user.EmailConfirmed);
                command.Parameters.AddWithValue("password", user.Password);
                command.Parameters.AddWithValue("id", user.Id);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }
    }
}