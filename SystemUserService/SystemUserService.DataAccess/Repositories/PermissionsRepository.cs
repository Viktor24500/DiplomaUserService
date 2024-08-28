using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Permissions;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.DataAccess.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {

        private readonly string? _connectionString;
        public PermissionsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
        }
        public async Task<Result<PermissionDTO>> CreatePermission(string name, string? description)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insertPermission", connection);

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
                    Result<PermissionDTO> result = new Result<PermissionDTO>();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("permissionDescription"));
                        }
                        result.Data = new PermissionDTO(
                            reader.GetInt32(reader.GetOrdinal("permissionId")),
                            reader.GetString(reader.GetOrdinal("permissionName")),
                            description
                            );
                    }
                    return result;
                }
            }
        }

        public async Task<Result<List<PermissionDTO>>> GetAllPermissions()
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getAllPermissions", connection);
                command.CommandType = CommandType.StoredProcedure;
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<List<PermissionDTO>> result = new Result<List<PermissionDTO>>();
                    result.Data = new List<PermissionDTO>();
                    while (reader.Read())
                    {
                        string? description;
                        if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("permissionDescription"));
                        }
                        PermissionDTO permission = new PermissionDTO(
                            reader.GetInt32(reader.GetOrdinal("permissionId")),
                            reader.GetString(reader.GetOrdinal("permissionName")),
                            description
                            );
                        result.Data.Add(permission);
                    }
                    return result;
                }
            }
        }

        public async Task<Result<PermissionDTO>> GetPermission(int id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getPermissionById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<PermissionDTO> result = new Result<PermissionDTO>();
                    while (reader.Read())
                    {
                        string? description;
                        if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("permissionDescription"));
                        }
                        result.Data = new PermissionDTO(
                            reader.GetInt32(reader.GetOrdinal("permissionId")),
                            reader.GetString(reader.GetOrdinal("permissionName")),
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

        public async Task<Result<PermissionDTO>> GetPermissionByName(string name)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getPermissionByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@name", name);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<PermissionDTO> result = new Result<PermissionDTO>();
                    while (reader.Read())
                    {
                        string? description;
                        if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("permissionDescription"));
                        }
                        result.Data = new PermissionDTO(
                            reader.GetInt32(reader.GetOrdinal("permissionId")),
                            reader.GetString(reader.GetOrdinal("permissionName")),
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

        public async Task<Result<PermissionDTO>> UpdatePermission(int id, string name, string? description)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                Result<PermissionDTO> result = new Result<PermissionDTO>();
                connection.Open();

                SqlCommand command = new SqlCommand("updatePermission", connection);
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
                    result.ErrorMessage = $"Permission with {id} not found";
                    return result;
                }
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
                        {
                            description = null;
                        }
                        else
                        {
                            description = reader.GetString(reader.GetOrdinal("permissionDescription"));
                        }
                        result.Data = new PermissionDTO(
                            reader.GetInt32(reader.GetOrdinal("permissionId")),
                            reader.GetString(reader.GetOrdinal("permissionName")),
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
