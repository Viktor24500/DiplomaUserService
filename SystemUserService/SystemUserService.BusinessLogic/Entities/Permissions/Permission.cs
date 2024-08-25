namespace SystemUserService.BusinessLogic.Entities.Permissions
{
    public class Permission
    {
        public Permission(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
