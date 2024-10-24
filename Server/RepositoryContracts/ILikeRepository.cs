﻿using Entities;

namespace RepositoryContracts;

public interface ILikeRepository
{
    Task<Like> AddLikeAsync(Like like);
    Task DeleteLikeAsync(int likeId);
    Task<Like> GetLikeByIdAsync(int id);
    IQueryable<Like> GetLikesForPost(int postId);
    IQueryable<Like> GetAllLikes();
}