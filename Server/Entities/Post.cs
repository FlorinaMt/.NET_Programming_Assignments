namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public User User { get; set; } = null!;
    private string? title, body;
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }

    private Post()
    {
    }

    public static Post getPost()
    {
        return new Post();
    }

    public string Title
    {
        get { return title; }
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The title cannot be empty.");
            title = value;
        }
    }

    public string Body
    {
        get => body;
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The body cannot be empty.");
            body = value;
        }
    }


    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        Post other = (Post)obj;
        if (other.User == User && other.Title.Equals(Title) &&
            other.Body.Equals(Body) && other.PostId == PostId)
            return true;
        return false;
    }
}