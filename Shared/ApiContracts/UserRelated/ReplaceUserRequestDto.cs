namespace ApiContracts;

public class ReplaceUserRequestDto
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}