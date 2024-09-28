namespace ApiContracts;

public class ReplacePostRequestDto
{
    public required int PostId { get; set; }
    public required int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}