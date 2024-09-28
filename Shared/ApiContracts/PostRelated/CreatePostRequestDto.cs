namespace ApiContracts;

public class CreatePostRequestDto
{
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int UserId { get; set; }
}