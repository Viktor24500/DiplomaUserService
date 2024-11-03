namespace SystemUserService.BusinessLogic.Parametrs.Roles
{
    public class RoleCreateParametrs
    {
        public RoleCreateParametrs(string name, string? description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
