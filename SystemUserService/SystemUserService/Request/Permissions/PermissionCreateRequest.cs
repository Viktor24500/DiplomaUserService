namespace SystemUserService.Request.Permissions
{
    public class PermissionCreateRequest
    {
        public PermissionCreateRequest(string name, string? description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
