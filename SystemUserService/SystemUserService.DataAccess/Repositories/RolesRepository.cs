using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.Repositories.Intefaces;
using SystemUserService.Common.Enums;

namespace SystemUserService.DataAccess.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly string? _connectionString;
        public RolesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
        }

        public async Task<Result<RoleDTO>> CreateRole(string name, string? description)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insertRole", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@name", name);
                if (description != null)
                {
                    command.Parameters.AddWithValue("@description", description);
                }
                else
                {
                    command.Parameters.AddWithValue("@description", DBNull.Value);
                }
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<RoleDTO> result = new Result<RoleDTO>();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        result.Data = new RoleDTO(
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            description
                            );
                    }
                    return result;
                }
            }
        }

        public async Task<Result<List<RoleDTO>>> GetAllRoles()
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getAllRoles", connection);
                command.CommandType = CommandType.StoredProcedure;
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<List<RoleDTO>> result = new Result<List<RoleDTO>>();
                    result.Data = new List<RoleDTO>();
                    while (reader.Read())
                    {
                        string? description;
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        RoleDTO role = new RoleDTO(
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            description
                            );
                        result.Data.Add(role);
                    }
                    return result;
                }
            }
        }

        public async Task<Result<RoleDTO>> GetRole(int id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getRoleById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<RoleDTO> result = new Result<RoleDTO>();
                    while (reader.Read())
                    {
                        string? description;
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        result.Data = new RoleDTO(
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            description
                            );
                    }
                    if (result.Data != null)
                    {
                        result.ErrorCode = (int)ErrorCodes.Success;
                        return result;
                    }
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    return result;
                }
            }
        }

        public async Task<Result<RoleDTO>> GetRoleByName(string name)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getRoleByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@name", name);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<RoleDTO> result = new Result<RoleDTO>();
                    while (reader.Read())
                    {
                        string? description;
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        result.Data = new RoleDTO(
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            description
                            );
                    }
                    if (result.Data != null)
                    {
                        result.ErrorCode = (int)ErrorCodes.Success;
                        return result;
                    }
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    return result;
                }
            }
        }

        public async Task<Result<RoleDTO>> UpdateRole(int id, string name, string? description)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                Result<RoleDTO> result = new Result<RoleDTO>();
                connection.Open();
                SqlCommand command = new SqlCommand("updateRole", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                if (description != null)
                {
                    command.Parameters.AddWithValue("@description", description);
                }
                else
                {
                    command.Parameters.AddWithValue("@description", DBNull.Value);
                }

                if (command.ExecuteNonQuery() <= 0)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"Role with {id} not found";
                    return result;
                }
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        result.Data = new RoleDTO(
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            description
                            );
                    }
                    result.ErrorCode = (int)ErrorCodes.Success;
                    return result;
                }
            }
        }
    }
}
