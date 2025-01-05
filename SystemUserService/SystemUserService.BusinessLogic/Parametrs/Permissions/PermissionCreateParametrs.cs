namespace SystemUserService.BusinessLogic.Parametrs.Permissions
{
    public class PermissionCreateParametrs
    {
        public PermissionCreateParametrs(string name, string? description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
