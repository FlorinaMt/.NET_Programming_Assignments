namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    private string? commentBody;

    public string CommentBody
    {
        get => commentBody;
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The comment cannot be empty.");
            commentBody = value;
        }
    }

}