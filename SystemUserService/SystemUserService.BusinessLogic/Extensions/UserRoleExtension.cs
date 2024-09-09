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
            List<UserRole> userRoleList = new List<UserRole>();
            Dictionary<int, UserRole> userRoleMap = new Dictionary<int, UserRole>();

            foreach (UserRoleDTO dto in userRoleDTOList)
            {
                Role role = new Role(
                    dto.RoleId, dto.RoleName, dto.RoleDescription
                );

                if (userRoleMap.ContainsKey(dto.UserRolesUserId))
                {
                    userRoleMap[dto.UserRolesUserId].Roles.Add(role);
                }
                else
                {
                    User user = new User(
                        dto.UserId, dto.Username, dto.UserPassword,
                        dto.Email, dto.FirstName, dto.LastName,
                        dto.FatherName, dto.DateRegistered, dto.LastLogin,
                        dto.TokenExpiration, dto.IsActive, dto.LastToken
                    );

                    List<Role> roles = new List<Role> { role };

                    UserRole userRole = new UserRole(
                        dto.UserRoleId,
                        dto.UserRolesUserId,
                        dto.UserRolesRoleId,
                        roles,
                        user
                    );

                    // Add the new UserRole to the dictionary
                    userRoleMap[dto.UserRoleId] = userRole;
                }
            }

            // Convert the dictionary values to a list
            userRoleList = userRoleMap.Values.ToList();

            return userRoleList;

        }
    }
}
