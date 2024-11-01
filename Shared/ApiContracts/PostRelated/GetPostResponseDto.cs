using ApiContracts.LikeRelated;

namespace ApiContracts;

public class GetPostResponseDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string AuthorUsername { get; set; }
    public int PostId { get; set; }
    public int LikesNo  { get; set; }
    public List<GetLikeDto> Likes { get; set; }
    public List<GetCommentResponseDto> Comments { get; set; }
}