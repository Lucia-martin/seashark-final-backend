namespace MyApp.Models
{
    public class Project
    {
        public Project(int id, string name, DateTime created)
        {
            Id = id;
            Name = name;
            Created = created;
            Todos = new List<Todo>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }
        public List<Todo> Todos { get; set; }
    }
}


