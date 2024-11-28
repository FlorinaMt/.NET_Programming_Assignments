using System.Diagnostics;

namespace Entities;

public class Comment
{
    public int CommentId { get; set; }
    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;
    private string? commentBody;

    private Comment()
    {
        
    }
    public static Comment getComment()
    {
        return new Comment();
    }

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



    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        Comment other=(Comment)obj;
        if (other.CommentBody.Equals(commentBody) && other.User==User && other.Post==Post && other.CommentId==CommentId)
            return true;
        return false;
    }
}