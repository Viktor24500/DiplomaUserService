namespace SystemUserService.BusinessLogic.Parametrs.RolePermission
{
	public class RolePermissionUpdateParameters
	{
		public RolePermissionUpdateParameters(int rolePermissionId, int roleId, int permissionId)
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
