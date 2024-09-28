namespace ApiContracts.ReactionRelated;

//used for response in both POST and GET 
public class GetReactionResponseDto
{
    public required int ReactionId { get; set; }
    public required int CommentId { get; set; }
    public required string AuthorUsername { get; set; }
    public required bool IsPositive { get; set; }
}