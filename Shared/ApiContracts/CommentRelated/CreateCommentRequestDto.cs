namespace ApiContracts;

public class CreateCommentRequestDto
{
    public int UserId { get; set; }
    public string Body { get; set; }
}