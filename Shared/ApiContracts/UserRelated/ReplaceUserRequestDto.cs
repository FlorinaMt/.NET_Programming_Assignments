namespace ApiContracts;

public class ReplaceUserRequestDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
}