namespace SystemUserService.Request.RolePermission
{
	public class RolePermissionCreateRequest
	{
		public RolePermissionCreateRequest(int roleId, List<int> permissionsId)
		{
			RoleId = roleId;
			PermissionsId = permissionsId;
		}

		public int RoleId { get; set; }
		public List<int> PermissionsId { get; set; }
	}
}
