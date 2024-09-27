namespace ApiContracts;

public class ReplaceCommentRequestDto
{
    public string Body { get; set; }
    public int CommentId { get; set; }
    public int UserId { get; set; }
}