using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace WebApplication1
{
    public class RoleStore : IRoleStore<IdentityRole>, IUserRoleStore<ApplicationUser>
    {
        private readonly string _connectionString = "host=127.0.0.1;port=5432;Username=postgres;password=password;database=spacestation";

        public RoleStore()
        {
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            IdentityRole role = await GetRole(roleName, cancellationToken);
            await InsertUserRole(user, Convert.ToInt16(role.Id), cancellationToken);
        }

        private async Task<IdentityRole> GetRole(string roleName, CancellationToken cancellationToken)
        {
            string sql = @"SELECT id, name, normalized_name FROM roles where roleName=@roleName;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("roleName", roleName);
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync())
                {
                    return new IdentityRole()
                    {
                        Id = reader.GetInt32(0).ToString(),
                        Name = reader.GetString(1),
                        NormalizedName = reader.GetString(2),
                    };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        private async Task InsertUserRole(ApplicationUser user, short roleId, CancellationToken cancellationToken)
        {
            string sql = @"INSERT INTO user_roles(user_id, role_user) VALUES (@user_id, @role_id);";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("user_id", user.Id);
                command.Parameters.AddWithValue("role_id", roleId);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception)
            {
            }
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            string sql = @"INSERT INTO roles(name, normalized_name) RETURNING ID;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("name", role.Name);
                command.Parameters.AddWithValue("normalized_name", role.NormalizedName);
                role.Id = Convert.ToInt16(await command.ExecuteScalarAsync(cancellationToken)).ToString();
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            string sql = @"DELETE FROM roles WHERE id = @id";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("id", role.Id);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            string sql = @"SELECT id, name, normalized_name FROM roles WHERE id = @id;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("id", roleId);
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return new IdentityRole()
                    {
                        Id = reader.GetInt16(0).ToString(),
                        Name = reader.GetString(1),
                        NormalizedName = reader.GetString(2),
                    };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            string sql = @"SELECT id, name, normalized_name FROM roles WHERE normalized_name = @normalized_name;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("normalized_name", normalizedRoleName);
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return new IdentityRole()
                    {
                        Id = reader.GetInt16(0).ToString(),
                        Name = reader.GetString(1),
                        NormalizedName = reader.GetString(2),
                    };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            string sql = @"UPDATE roles SET name=@name, normalized_name=@normalized_name) WHERE ID=@id;";
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                await connection.OpenAsync(cancellationToken);
                NpgsqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("name", role.Name);
                command.Parameters.AddWithValue("normalized_name", role.NormalizedName);
                command.Parameters.AddWithValue("id", role.Id);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ApplicationUser> IUserStore<ApplicationUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ApplicationUser> IUserStore<ApplicationUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}