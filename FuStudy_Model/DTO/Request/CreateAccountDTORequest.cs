namespace FuStudy_Model.DTO.Request;

public class CreateAccountDTORequest
{
    public required string Fullname { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string Avatar { get; set; }
    public string Gender { get; set; }
    public long RoleId { get; set; }
    public string IdentityCard { get; set; }
    public string phone { get; set; }
}