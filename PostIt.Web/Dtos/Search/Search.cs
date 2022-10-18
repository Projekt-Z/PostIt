namespace PostIt.Web.Dtos.Search
{
    public class Search
    {
        public IEnumerable<Models.Post>? Posts { get; set; } = default!;
        public IEnumerable<Models.User>? Users { get; set; } = default!;
    }
}
