namespace PostIt.Web.Dtos.Authentication;

public class CreateConfirm
{
    public string? Email { get; set; }
    public int Code { get; set; }
}