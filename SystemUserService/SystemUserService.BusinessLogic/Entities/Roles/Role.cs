namespace SystemUserService.BusinessLogic.Entities.Roles
{
    public class Role
    {
        public Role(int id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
