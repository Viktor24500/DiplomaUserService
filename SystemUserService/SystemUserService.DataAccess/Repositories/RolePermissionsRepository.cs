using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.RolesPermissions;
using SystemUserService.DataAccess.Repositories.Intefaces;
using SystemUserService.Common.Enums;

namespace SystemUserService.DataAccess.Repositories
{
	public class RolePermissionsRepository : IRolePermissionsRepository
	{
		private readonly string? _connectionString;
		public RolePermissionsRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
		}
		public async Task<Result> CreateRolePermissions(int roleId, List<int> permissionsId)
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();
				using (SqlCommand command = new SqlCommand("insertRolesPermissions", connection, transaction))
				{
					//SqlCommand command = connection.CreateCommand();
					//command.Transaction = transaction;
					//command = new SqlCommand("insertRolesPermissions", connection);
					command.CommandType = CommandType.StoredProcedure;

					Result result = new Result();
					try
					{
						foreach (int permissionId in permissionsId)
						{
							command.Parameters.Clear();
							command.Parameters.AddWithValue("@roleId", roleId);
							command.Parameters.AddWithValue("@permissionId", permissionId);
							if (await command.ExecuteNonQueryAsync() <= 0)
							{
								result.ErrorCode = (int)ErrorCodes.InternalServerError;
								await transaction.RollbackAsync();
								return result;
							}
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

		public async Task<Result<List<RolePermissionsDTO>>> GetAllRolePermissions()
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				SqlCommand command = new SqlCommand("getAllRolesPermissions", connection);
				command.CommandType = CommandType.StoredProcedure;
				await using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					Result<List<RolePermissionsDTO>> result = new Result<List<RolePermissionsDTO>>
					{
						Data = new List<RolePermissionsDTO>()
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

						RolePermissionsDTO rolePermissions = new RolePermissionsDTO(
							reader.GetInt32(reader.GetOrdinal("rolePermissionId")),
							reader.GetInt32(reader.GetOrdinal("rolePermissionRoleId")),
							reader.GetInt32(reader.GetOrdinal("rolePermissionPermissionId")),
							reader.GetInt32(reader.GetOrdinal("roleId")),
							reader.GetString(reader.GetOrdinal("roleName")),
							roleDescription,
							reader.GetInt32(reader.GetOrdinal("permissionId")),
							reader.GetString(reader.GetOrdinal("permissionName")),
							permissionDescription
						);

						result.Data.Add(rolePermissions);
					}

					return result;
				}
			}
		}

		public async Task<Result<RolePermissionsDTO>> GetRolePermissionsByRoleId(int id)
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand("getAllRolesPermissionsByRoleId", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@roleId", id);
				await using (SqlDataReader reader = command.ExecuteReader())
				{
					Result<RolePermissionsDTO> result = new Result<RolePermissionsDTO>();
					while (reader.Read())
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

						result.Data = new RolePermissionsDTO(
							reader.GetInt32(reader.GetOrdinal("rolePermissionId")),
							reader.GetInt32(reader.GetOrdinal("rolePermissionRoleId")),
							reader.GetInt32(reader.GetOrdinal("rolePermissionPermissionId")),
							reader.GetInt32(reader.GetOrdinal("roleId")),
							reader.GetString(reader.GetOrdinal("roleName")),
							roleDescription,
							reader.GetInt32(reader.GetOrdinal("permissionId")),
							reader.GetString(reader.GetOrdinal("permissionName")),
							permissionDescription
						);

					}
					if (result.Data == null)
					{
						result.ErrorCode = (int)ErrorCodes.NotFound;
						return result;
					}
					return result;
				}
			}
		}

		public async Task<Result> UpdateRolePermissions(int rolePermissionId, int roleId, int permissionId)
		{
			await using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand("updateRolesPermissions", connection);
				command.CommandType = CommandType.StoredProcedure;

				Result result = new Result();
				command.Parameters.AddWithValue("@rolePermissionId", rolePermissionId);
				command.Parameters.AddWithValue("@roleId", roleId);
				command.Parameters.AddWithValue("@permissionId", permissionId);
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
