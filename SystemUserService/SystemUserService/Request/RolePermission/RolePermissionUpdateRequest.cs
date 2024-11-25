namespace SystemUserService.Request.RolePermission
{
	public class RolePermissionUpdateRequest
	{
		public RolePermissionUpdateRequest(int rolePermissionId, int roleId, int permissionId)
		{
			RolePermissionId = rolePermissionId;
			RoleId = roleId;
			PermissionId = permissionId;
		}

		public int RolePermissionId { get; set; }
		public int RoleId { get; set; }
		public int PermissionId { get; set; }
	}
}
