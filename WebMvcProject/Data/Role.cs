namespace WebMvcProject.Data
{
    public class Role
    {

        public string Id { get; init; }
        public string Name { get; set; }

        public List<User> Users { get; set; }

        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }


    }
}
