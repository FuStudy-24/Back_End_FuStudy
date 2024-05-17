namespace FuStudy_Model.DTO.Request;

public class CreateAccountDTORequest
{
    public required string Fullname { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required DateOnly dob { get; set; }
    public string IdentityCard { get; set; }
    public string phone { get; set; }
}