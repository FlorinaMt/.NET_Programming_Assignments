namespace Entities;

public class Like
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string ToString()
    {
        return($"LikeId = {LikeId}, UserId = {UserId}, PostId = {PostId}");
    }
}