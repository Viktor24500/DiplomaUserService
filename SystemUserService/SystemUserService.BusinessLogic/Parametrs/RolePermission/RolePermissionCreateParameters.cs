namespace SystemUserService.BusinessLogic.Parametrs.RolePermission
{
	public class RolePermissionCreateParameters
	{
		public RolePermissionCreateParameters(int roleId, List<int> permissionsId)
		{
			RoleId = roleId;
			PermissionsId = permissionsId;
		}

		public int RoleId { get; set; }
		public List<int> PermissionsId { get; set; }
	}
}
