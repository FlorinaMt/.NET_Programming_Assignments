namespace ApiContracts.LikeRelated;

public class AddLikeRequestDto
{
    public required int UserId { get; set; }
    public required int PostId { get; set; }
}