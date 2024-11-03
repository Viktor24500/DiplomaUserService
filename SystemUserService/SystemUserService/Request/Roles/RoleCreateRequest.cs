namespace SystemUserService.Request.Roles
{
    public class RoleCreateRequest
    {
        public RoleCreateRequest(string name, string? description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
