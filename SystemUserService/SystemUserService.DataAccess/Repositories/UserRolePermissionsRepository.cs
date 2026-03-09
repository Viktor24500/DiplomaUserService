using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.UsersRolesPermissions;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.DataAccess.Repositories
{
	public class UserRolePermissionsRepository : IUserRolePermissionsRepository
	{
		private readonly string? _connectionString;
		public UserRolePermissionsRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
		}

		public async Task<Result<List<UserRolePermissionDTO>>> GetAllUserRolePermissions()
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				SqlCommand command = new SqlCommand("getAllUsersRolesPermissions", connection);
				command.CommandType = CommandType.StoredProcedure;
				await using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					Result<List<UserRolePermissionDTO>> result = new Result<List<UserRolePermissionDTO>>
					{
						Data = new List<UserRolePermissionDTO>()
					};

					while (await reader.ReadAsync())
					{
						string? roleDescription;
						if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
						{
							roleDescription = null;
						}
						else
						{
							roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
						}

						string? permissionDescription;
						if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
						{
							permissionDescription = null;
						}
						else
						{
							permissionDescription = reader.GetString(reader.GetOrdinal("permissionDescription"));
						}
						string? comments;
						if (reader.IsDBNull(reader.GetOrdinal("comments")))
						{
							comments = null;
						}
						else
						{
							comments = reader.GetString(reader.GetOrdinal("comments"));
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
						UserRolePermissionDTO userRolePermission = new UserRolePermissionDTO(
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
							comments,
							reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
							lastLogin,
							tokenExpiration,
							lastToken,
							reader.GetBoolean(reader.GetOrdinal("isActive")),
							reader.GetString(reader.GetOrdinal("phoneNumber")),
							reader.GetInt32(reader.GetOrdinal("permissionId")),
							reader.GetString(reader.GetOrdinal("permissionName")),
							permissionDescription
						);

						result.Data.Add(userRolePermission);
					}

					return result;
				}
			}

		}

		public async Task<Result<List<UserRolePermissionDTO>>> GetUserRolePermissionsByPermissionId(int id)
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				SqlCommand command = new SqlCommand("getAllUsersRolesPermissionsByPermissionId", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@permissionId", id);
				await using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					Result<List<UserRolePermissionDTO>> result = new Result<List<UserRolePermissionDTO>>
					{
						Data = new List<UserRolePermissionDTO>()
					};

					while (await reader.ReadAsync())
					{
						string? roleDescription;
						if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
						{
							roleDescription = null;
						}
						else
						{
							roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
						}

						string? permissionDescription;
						if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
						{
							permissionDescription = null;
						}
						else
						{
							permissionDescription = reader.GetString(reader.GetOrdinal("permissionDescription"));
						}
						string? comments;
						if (reader.IsDBNull(reader.GetOrdinal("comments")))
						{
							comments = null;
						}
						else
						{
							comments = reader.GetString(reader.GetOrdinal("comments"));
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
						UserRolePermissionDTO userRolePermission = new UserRolePermissionDTO(
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
							comments,
							reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
							lastLogin,
							tokenExpiration,
							lastToken,
							reader.GetBoolean(reader.GetOrdinal("isActive")),
							reader.GetString(reader.GetOrdinal("phoneNumber")),
							reader.GetInt32(reader.GetOrdinal("permissionId")),
							reader.GetString(reader.GetOrdinal("permissionName")),
							permissionDescription
						);

						result.Data.Add(userRolePermission);
					}

					return result;
				}
			}
		}

		public async Task<Result<List<UserRolePermissionDTO>>> GetUserRolePermissionsByRoleId(int id)
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				SqlCommand command = new SqlCommand("getAllUsersRolesPermissionsByRoleId", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@roleId", id);
				await using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					Result<List<UserRolePermissionDTO>> result = new Result<List<UserRolePermissionDTO>>
					{
						Data = new List<UserRolePermissionDTO>()
					};

					while (await reader.ReadAsync())
					{
						string? roleDescription;
						if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
						{
							roleDescription = null;
						}
						else
						{
							roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
						}

						string? permissionDescription;
						if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
						{
							permissionDescription = null;
						}
						else
						{
							permissionDescription = reader.GetString(reader.GetOrdinal("permissionDescription"));
						}
						string? comments;
						if (reader.IsDBNull(reader.GetOrdinal("comments")))
						{
							comments = null;
						}
						else
						{
							comments = reader.GetString(reader.GetOrdinal("comments"));
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
						UserRolePermissionDTO userRolePermission = new UserRolePermissionDTO(
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
							comments,
							reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
							lastLogin,
							tokenExpiration,
							lastToken,
							reader.GetBoolean(reader.GetOrdinal("isActive")),
							reader.GetString(reader.GetOrdinal("phoneNumber")),
							reader.GetInt32(reader.GetOrdinal("permissionId")),
							reader.GetString(reader.GetOrdinal("permissionName")),
							permissionDescription
						);

						result.Data.Add(userRolePermission);
					}

					return result;
				}
			}
		}

		public async Task<Result<List<UserRolePermissionDTO>>> GetUserRolePermissionsByUserId(int id)
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				SqlCommand command = new SqlCommand("getAllUsersRolesPermissionsByUserId", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@userId", id);
				await using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					Result<List<UserRolePermissionDTO>> result = new Result<List<UserRolePermissionDTO>>
					{
						Data = new List<UserRolePermissionDTO>()
					};

					while (await reader.ReadAsync())
					{
						string? roleDescription;
						if (reader.IsDBNull(reader.GetOrdinal("roleDescription")))
						{
							roleDescription = null;
						}
						else
						{
							roleDescription = reader.GetString(reader.GetOrdinal("roleDescription"));
						}

						string? permissionDescription;
						if (reader.IsDBNull(reader.GetOrdinal("permissionDescription")))
						{
							permissionDescription = null;
						}
						else
						{
							permissionDescription = reader.GetString(reader.GetOrdinal("permissionDescription"));
						}
						string? comments;
						if (reader.IsDBNull(reader.GetOrdinal("comments")))
						{
							comments = null;
						}
						else
						{
							comments = reader.GetString(reader.GetOrdinal("comments"));
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
						UserRolePermissionDTO userRolePermission = new UserRolePermissionDTO(
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
							comments,
							reader.GetDateTime(reader.GetOrdinal("dateRegistered")),
							lastLogin,
							tokenExpiration,
							lastToken,
							reader.GetBoolean(reader.GetOrdinal("isActive")),
							reader.GetString(reader.GetOrdinal("phoneNumber")),
							reader.GetInt32(reader.GetOrdinal("permissionId")),
							reader.GetString(reader.GetOrdinal("permissionName")),
							permissionDescription
						);

						result.Data.Add(userRolePermission);
					}

					return result;
				}
			}
		}
	}
}
