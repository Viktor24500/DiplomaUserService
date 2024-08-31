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
            var role = new Role(dto.RoleId, dto.RoleName, dto.RoleDescription);
            var user = new User(dto.UserId, dto.Username, dto.UserPassword, dto.Email, dto.FirstName, dto.LastName, dto.FatherName, dto.DateRegistered, dto.LastLogin, dto.IsActive);
            var permission = new Permission(dto.PermissionId, dto.PermissionName, dto.PermissionDescription);

            return new UserRolePermissions(
                dto.UserRoleId,
                dto.UserRolesUserId,
                dto.UserRolesRoleId,
                role,
                user,
                permission
            );
        }
        public static List<UserRolePermissions> MapToUserRolePermissionsCollection(this List<UserRolePermissionDTO> dtoList)
        {
            // Create a new list to hold the mapped UserRolePermissions objects
            var userRolePermissionsList = new List<UserRolePermissions>();

            // Iterate over each UserRolePermissionDTO in the input list
            foreach (var dto in dtoList)
            {
                // Create the Role, User, and Permission instances from the DTO
                Role role = new Role(dto.RoleId, dto.RoleName, dto.RoleDescription);
                User user = new User(dto.UserId, dto.Username, dto.UserPassword, dto.Email, dto.FirstName, dto.LastName, dto.FatherName, dto.DateRegistered, dto.LastLogin, dto.IsActive);
                Permission permission = new Permission(dto.PermissionId, dto.PermissionName, dto.PermissionDescription);

                // Map the DTO to a UserRolePermissions object
                var userRolePermission = new UserRolePermissions(
                    dto.UserRoleId,
                    dto.UserRolesUserId,
                    dto.UserRolesRoleId,
                    role,
                    user,
                    permission
                );

                // Add the mapped UserRolePermissions object to the list
                userRolePermissionsList.Add(userRolePermission);
            }

            return userRolePermissionsList;
        }
    }
}
