using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.DataAccess.DTO.UsersRoles;

namespace SystemUserService.BusinessLogic.Extensions
{
	public static class UserRoleExtension
	{
		public static UserRole MapToUserRole(this UserRoleDTO userRoleDTO)
		{
			User user = new User(
				userRoleDTO.UserId, userRoleDTO.Username, userRoleDTO.UserPassword,
				userRoleDTO.Email, userRoleDTO.FirstName, userRoleDTO.LastName,
				userRoleDTO.Comment, userRoleDTO.DateRegistered, userRoleDTO.LastLogin,
				userRoleDTO.TokenExpiration, userRoleDTO.IsActive, userRoleDTO.LastToken, userRoleDTO.PhoneNumber
			);

			Role role = new Role(
				userRoleDTO.RoleId, userRoleDTO.RoleName, userRoleDTO.RoleDescription
			);


			return new UserRole(
				userRoleDTO.UserRoleId,
				userRoleDTO.UserRolesUserId,
				userRoleDTO.UserRolesRoleId,
				//roles,
				role,
				user
			);
		}
		public static List<UserRole> MapToUserRoleCollection(this List<UserRoleDTO> userRoleDTOList)
		{
			List<UserRole> userRoleList = new List<UserRole>();

			foreach (UserRoleDTO dto in userRoleDTOList)
			{
				// Create the Role object
				Role role = new Role(
					dto.RoleId,
					dto.RoleName,
					dto.RoleDescription
				);

				// Create the User object
				User user = new User(
					dto.UserId, dto.Username, dto.UserPassword,
					dto.Email, dto.FirstName,
					dto.LastName, dto.Comment,
					dto.DateRegistered, dto.LastLogin,
					dto.TokenExpiration, dto.IsActive, dto.LastToken, dto.PhoneNumber
				);

				// Create the UserRole object
				UserRole userRole = new UserRole(
					dto.UserRoleId,
					dto.UserRolesUserId,
					dto.UserRolesRoleId,
					role,
					user
				);

				// Add the UserRole object to the list
				userRoleList.Add(userRole);
			}

			return userRoleList;

		}
	}
}
