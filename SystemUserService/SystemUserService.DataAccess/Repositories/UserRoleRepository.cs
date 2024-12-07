using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.UsersRoles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.DataAccess.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly string? _connectionString;
        public UserRoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
        }

        public async Task<Result> CreateUserRole(int userId, int roleId)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (SqlCommand command = new SqlCommand("insertUserRole", connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    Result result = new Result();
                    try
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@roleId", roleId);
                        if (await command.ExecuteNonQueryAsync() <= 0)
                        {
                            result.ErrorCode = (int)ErrorCodes.InternalServerError;
                            await transaction.RollbackAsync();
                            return result;
                        }
                        await transaction.CommitAsync();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<Result<List<UserRoleDTO>>> GetAllUsersRoles()
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getAllUsersRoles", connection);
                command.CommandType = CommandType.StoredProcedure;
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<List<UserRoleDTO>> result = new Result<List<UserRoleDTO>>();
                    result.Data = new List<UserRoleDTO>();

                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        string? roleDescription;
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            roleDescription = null;
                        }
                        else
                        {
                            roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        UserRoleDTO userRole = new UserRoleDTO(
                            reader.GetInt32(reader.GetOrdinal("userRoleId")),
                            reader.GetInt32(reader.GetOrdinal("userRolesUserId")),
                            reader.GetInt32(reader.GetOrdinal("userRolesRoleId")),
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            roleDescription,
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            tokenExpiration,
                            lastToken,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                        );

                        result.Data.Add(userRole);
                    }

                    return result;
                }
            }
        }

        public async Task<Result<List<UserRoleDTO>>> GetUserRoleByRoleId(int id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUsersRolesByRoleId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roleId", id);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<List<UserRoleDTO>> result = new Result<List<UserRoleDTO>>();
                    result.Data = new List<UserRoleDTO>();

                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        string? roleDescription;
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            roleDescription = null;
                        }
                        else
                        {
                            roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        UserRoleDTO userRole = new UserRoleDTO(
                            reader.GetInt32(reader.GetOrdinal("userRoleId")),
                            reader.GetInt32(reader.GetOrdinal("userRolesUserId")),
                            reader.GetInt32(reader.GetOrdinal("userRolesRoleId")),
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            roleDescription,
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            tokenExpiration,
                            lastToken,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                        );

                        result.Data.Add(userRole);
                    }

                    return result;
                }
            }
        }

        public async Task<Result<UserRoleDTO>> GetUserRoleByUserId(int id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getUsersRolesByUserId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", id);
                await using (SqlDataReader reader = command.ExecuteReader())
                {
                    Result<UserRoleDTO> result = new Result<UserRoleDTO>();

                    while (reader.Read())
                    {
                        string? fatherName;
                        if (reader.IsDBNull(reader.GetOrdinal("fatherName")))
                        {
                            fatherName = null;
                        }
                        else
                        {
                            fatherName = reader.GetString(reader.GetOrdinal("fatherName"));
                        }
                        DateTime? lastLogin;
                        if (reader.IsDBNull(reader.GetOrdinal("lastLogin")))
                        {
                            lastLogin = null;
                        }
                        else
                        {
                            lastLogin = reader.GetDateTime(reader.GetOrdinal("lastLogin"));
                        }
                        string? roleDescription;
                        if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
                        {
                            roleDescription = null;
                        }
                        else
                        {
                            roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
                        }
                        string? lastToken;
                        if (reader.IsDBNull(reader.GetOrdinal("lastToken")))
                        {
                            lastToken = null;
                        }
                        else
                        {
                            lastToken = reader.GetString(reader.GetOrdinal("lastToken"));
                        }
                        DateTime? tokenExpiration;
                        if (reader.IsDBNull(reader.GetOrdinal("tokenExpiration")))
                        {
                            tokenExpiration = null;
                        }
                        else
                        {
                            tokenExpiration = reader.GetDateTime(reader.GetOrdinal("tokenExpiration"));
                        }
                        result.Data = new UserRoleDTO(
                            reader.GetInt32(reader.GetOrdinal("userRoleId")),
                            reader.GetInt32(reader.GetOrdinal("userRolesUserId")),
                            reader.GetInt32(reader.GetOrdinal("userRolesRoleId")),
                            reader.GetInt32(reader.GetOrdinal("roleId")),
                            reader.GetString(reader.GetOrdinal("roleName")),
                            roleDescription,
                            reader.GetInt32(reader.GetOrdinal("userId")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("userPassword")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("firstName")),
                            reader.GetString(reader.GetOrdinal("lastName")),
                            fatherName,
                            reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
                            lastLogin,
                            tokenExpiration,
                            lastToken,
                            reader.GetBoolean(reader.GetOrdinal("isActive"))
                        );
                    }

                    return result;
                }
            }
        }

        public async Task<Result> UpdateUserRole(int userRoleId, int userId, int roleId)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("updateUserRole", connection);
                command.CommandType = CommandType.StoredProcedure;

                Result result = new Result();
                command.Parameters.AddWithValue("@userRoleId", userRoleId);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@roleId", roleId);
                if (await command.ExecuteNonQueryAsync() <= 0)
                {
                    result.ErrorCode = (int)ErrorCodes.InternalServerError;
                    return result;
                }
                return result;
            }
        }
    }
}
