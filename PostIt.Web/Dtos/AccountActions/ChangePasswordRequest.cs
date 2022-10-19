namespace PostIt.Web.Dtos.AccountActions
{
    public class ChangePasswordRequest
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
