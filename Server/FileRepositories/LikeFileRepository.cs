using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class LikeFileRepository:ILikeRepository
{
    private ILikeRepository _likeRepositoryImplementation;
    public Task<Like> AddLikeAsync(Like like)
    {
        return _likeRepositoryImplementation.AddLikeAsync(like);
    }

    public Task DeleteLikeAsync(Like like)
    {
        return _likeRepositoryImplementation.DeleteLikeAsync(like);
    }

    public Task<Like> GetLikeByIdAsync(int id)
    {
        return _likeRepositoryImplementation.GetLikeByIdAsync(id);
    }

    public IQueryable<Like> GetLikesForPost(int postId)
    {
        return _likeRepositoryImplementation.GetLikesForPost(postId);
    }

    public IQueryable<Like> GetAllLikes()
    {
        return _likeRepositoryImplementation.GetAllLikes();
    }
}