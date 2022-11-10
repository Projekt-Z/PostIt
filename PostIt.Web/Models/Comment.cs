namespace PostIt.Web.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public string TimeAdded { get; set; } = default!;
        public bool? Reply { get; set; } = false;
        
        public List<Comment>? Comments { get; set; }
        public User Author { get; set; } = default!;
        public Post Post { get; set; } = default!;
    }
}
