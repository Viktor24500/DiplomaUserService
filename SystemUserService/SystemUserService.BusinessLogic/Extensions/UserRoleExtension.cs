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
            List<Role> roles = new List<Role>
            {
                   new Role(
                        userRoleDTO.RoleId, userRoleDTO.RoleName, userRoleDTO.RoleDescription
                    )
            };

            User user = new User(
                userRoleDTO.UserId, userRoleDTO.Username, userRoleDTO.UserPassword,
                userRoleDTO.Email, userRoleDTO.FirstName, userRoleDTO.LastName,
                userRoleDTO.FatherName, userRoleDTO.DateRegistered, userRoleDTO.LastLogin,
                userRoleDTO.TokenExpiration, userRoleDTO.IsActive, userRoleDTO.LastToken
            );

            return new UserRole(
                userRoleDTO.UserRoleId,
                userRoleDTO.UserRolesUserId,
                userRoleDTO.UserRolesRoleId,
                roles,
                user
            );
        }
        public static List<UserRole> MapToUserRoleCollection(this List<UserRoleDTO> userRoleDTOList)
        {
            // Create a new list to hold the mapped UserRole objects
            List<UserRole> userRoleList = new List<UserRole>();

            // Iterate over each UserRoleDTO in the input list
            foreach (var dto in userRoleDTOList)
            {
                List<Role> roles = new List<Role>
                {
                   new Role(
                        dto.RoleId, dto.RoleName, dto.RoleDescription
                    )
                };
                User user = new User(
                    dto.UserId, dto.Username, dto.UserPassword,
                    dto.Email, dto.FirstName, dto.LastName,
                    dto.FatherName, dto.DateRegistered, dto.LastLogin,
                     dto.TokenExpiration, dto.IsActive, dto.LastToken
                );

                // Map the DTO to a UserRole object
                UserRole userRole = new UserRole(
                    dto.UserRoleId,
                    dto.UserRolesUserId,
                    dto.UserRolesRoleId,
                    roles,
                    user
                );

                // Add the mapped UserRole object to the list
                userRoleList.Add(userRole);
            }
            return userRoleList;
        }
    }
}
