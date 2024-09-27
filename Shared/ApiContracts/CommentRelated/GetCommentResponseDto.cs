namespace ApiContracts;

public class GetCommentResponseDto
{
    public string AuthorUsername { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
}