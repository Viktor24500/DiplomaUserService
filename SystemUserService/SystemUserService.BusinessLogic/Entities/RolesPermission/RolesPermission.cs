using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Entities.Roles;

namespace SystemUserService.BusinessLogic.Entities.RolesPermission
{
    public class RolesPermission
    {
        public RolesPermission(int rolePermissionId, int rolePermissionroleId, int rolePermissionPermissionId, int roleId,
            string roleName, string? roleDescription, List<Permission> permissions)
        {
            RolePermissionId = rolePermissionId;
            RoleId = rolePermissionroleId;
            Role = new Role(roleId, roleName, roleDescription);
            PermissionId = rolePermissionPermissionId;
            Permission = permissions;
        }
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }
        public List<Permission> Permission { get; set; }
    }
}
