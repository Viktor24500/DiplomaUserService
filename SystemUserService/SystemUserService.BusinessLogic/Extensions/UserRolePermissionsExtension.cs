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
            List<Role> roles = new List<Role>
            {
                new Role(
                dto.RoleId, dto.RoleName, dto.RoleDescription
                )
            };
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
                roles,
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
                User user = new User(
                    dto.UserId, dto.Username, dto.UserPassword, dto.Email, dto.FirstName, dto.LastName,
                    dto.FatherName, dto.DateRegistered, dto.LastLogin, dto.TokenExpiration, dto.IsActive, dto.LastToken
                );

                Role role = new Role(
                    dto.RoleId, dto.RoleName, dto.RoleDescription
                );

                Permission permission = new Permission(
                    dto.PermissionId, dto.PermissionName, dto.PermissionDescription
                );

                if (userRolePermissionsMap.ContainsKey(dto.UserRoleId))
                {
                    var userRolePermissions = userRolePermissionsMap[dto.UserRoleId];
                    if (!userRolePermissions.Role.Contains(role))
                    {
                        userRolePermissions.Role.Add(role);
                    }
                    if (!userRolePermissions.Permission.Contains(permission))
                    {
                        userRolePermissions.Permission.Add(permission);
                    }
                }
                else
                {
                    //if (roles == null)
                    //{
                    //    roles = new List<Role>();
                    //}
                    //if (permissions == null)
                    //{
                    //    permissions = new List<Permission>();
                    //}
                    //if (!roles.Contains(role))
                    //{
                    //    roles = new List<Role> { role };
                    //}
                    //if (!permissions.Contains(permission))
                    //{
                    //    permissions = new List<Permission> { permission };
                    //}
                    List<Role> roles = new List<Role> { role };
                    List<Permission> permissions = new List<Permission> { permission };
                    UserRolePermissions userRolePermissions = new UserRolePermissions(
                        dto.UserRoleId,
                        dto.UserRolesUserId,
                        dto.UserRolesRoleId,
                        roles,
                        user,
                        permissions
                    );

                    userRolePermissionsMap[dto.UserRoleId] = userRolePermissions;
                }
            }

            userRolePermissionsList = userRolePermissionsMap.Values.ToList();

            return userRolePermissionsList;

        }
    }
}
