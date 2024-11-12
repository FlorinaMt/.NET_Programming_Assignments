namespace ApiContracts;

public class GetCommentResponseDto
{
    public required string AuthorUsername { get; set; }
    public int AuthorId { get; set; }
    public required string Body { get; set; }
    public required int PostId { get; set; }
    public required int CommentId { get; set; }
}