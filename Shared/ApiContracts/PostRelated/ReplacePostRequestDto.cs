namespace ApiContracts;

public class ReplacePostRequestDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}