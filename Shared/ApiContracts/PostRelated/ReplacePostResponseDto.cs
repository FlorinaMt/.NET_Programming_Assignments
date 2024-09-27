namespace ApiContracts;

public class ReplacePostResponseDto
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string AuthorUsername { get; set; }
    public int LikesNo{ get; set; }
    public List<Object> Comments { get; set; }
}