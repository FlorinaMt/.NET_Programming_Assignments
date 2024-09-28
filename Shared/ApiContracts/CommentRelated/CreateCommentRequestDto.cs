namespace ApiContracts;

public class CreateCommentRequestDto
{
    public required int UserId { get; set; }
    public required string Body { get; set; }
    public required int PostId { get; set; }
}