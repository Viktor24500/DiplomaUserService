using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Entities.UsersRolesPermissions;
using SystemUserService.DataAccess.DTO.UsersRolesPermissions;

namespace SystemUserService.BusinessLogic.Extensions
{
	public static class UserRolePermissionsExtension
	{
		public static UserRolePermissions MapToUserRolePermissions(this UserRolePermissionDTO dto)
		{
			Role role = new Role(
				dto.RoleId, dto.RoleName, dto.RoleDescription
			);

			User user = new User(dto.UserId, dto.Username, dto.UserPassword, dto.Email,
				dto.FirstName, dto.LastName, dto.FatherName, dto.DateRegistered,
				dto.LastLogin, dto.TokenExpiration, dto.IsActive, dto.LastToken);

			List<Permission> permissions = new List<Permission>
			{
				new Permission(
					dto.PermissionId, dto.PermissionName, dto.PermissionDescription
				)
			};

			return new UserRolePermissions(
				dto.UserRoleId,
				dto.UserRolesUserId,
				dto.UserRolesRoleId,
				//roles,
				role,
				user,
				permissions
			);
		}
		public static List<UserRolePermissions> MapToUserRolePermissionsCollection(this List<UserRolePermissionDTO> userRolePermissionDTOList)
		{
			List<UserRolePermissions> userRolePermissionsList = new List<UserRolePermissions>();
			Dictionary<int, UserRolePermissions> userRolePermissionsMap = new Dictionary<int, UserRolePermissions>();

			foreach (UserRolePermissionDTO dto in userRolePermissionDTOList)
			{
				// Create the User object
				User user = new User(
					dto.UserId, dto.Username, dto.UserPassword,
					dto.Email, dto.FirstName,
					dto.LastName, dto.FatherName,
					dto.DateRegistered, dto.LastLogin,
					dto.TokenExpiration, dto.IsActive,
					dto.LastToken
				);

				// Create the Role object
				Role role = new Role(
					dto.RoleId,
					dto.RoleName,
					dto.RoleDescription
				);

				// Create the Permission object
				Permission permission = new Permission(
					dto.PermissionId,
					dto.PermissionName,
					dto.PermissionDescription
				);

				if (userRolePermissionsMap.ContainsKey(dto.UserRoleId))
				{
					var userRolePermissions = userRolePermissionsMap[dto.UserRoleId];
					if (!userRolePermissions.Permission.Contains(permission))
					{
						userRolePermissions.Permission.Add(permission);
					}
				}
				else
				{
					// Create a new UserRolePermissions object with a single role and permissions
					List<Permission> permissions = new List<Permission> { permission };

					UserRolePermissions userRolePermissions = new UserRolePermissions(
						dto.UserRoleId,
						dto.UserRolesUserId,
						dto.UserRolesRoleId,
						role,
						user,
						permissions
					);

					userRolePermissionsMap[dto.UserRoleId] = userRolePermissions;
				}
			}

			// Convert the dictionary values to a list
			userRolePermissionsList = userRolePermissionsMap.Values.ToList();

			return userRolePermissionsList;

		}
	}
}
