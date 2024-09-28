namespace ApiContracts;

public class ReplaceCommentRequestDto
{
    public required string Body { get; set; }
    public required int CommentId { get; set; }
    public required int UserId { get; set; }
}