namespace MyApp.Models
{
    public class Todo
    {
        public Todo(int id, string text, string username, DateTime created, bool completed, string tag)
        {
            Id = id;
            Text = text;
            Username = username;
            Created = created;
            Completed = completed;
            Tag = tag;
        }

        public int Id { get; set; }
        public string? Text { get; set; }
        public bool Completed { get; set; }
        public string? Username { get; set; }
        public string? Tag { get; set; }
        public DateTime Created { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}



