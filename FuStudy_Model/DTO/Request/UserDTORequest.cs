using System;

namespace FuStudy_Model.DTO.Request;

public class UserDTORequest
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string Fullname { get; set; }
    public string IdentityCard { get; set; }
    public string Gender { get; set; }
    public string Avatar { get; set; }
    public DateTime Dob { get; set; }
    public string Phone { get; set; }
    public bool Status { get; set; }
    public long RoleId { get; set; }
}