namespace PostIt.Web.Models
{
    public class BlockedUser
    {
        public int Id { get; set; }
        public Guid BlockedUserId { get; set; }
    }
}
