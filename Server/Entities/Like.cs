namespace Entities;

public class Like
{
    public int LikeId { get; set; }
    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;

    private Like()
    {
        
    }
    public static Like getLike()
    {
        return new Like();
    }
}