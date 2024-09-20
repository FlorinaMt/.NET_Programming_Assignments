﻿using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class LikeFileRepository:ILikeRepository
{
    private readonly string filePath = "likes.json";

    public async Task<Like> AddLikeAsync(Like like)
    {
        List<Like> likes = await LoadLikesAsync();
        int maxId = likes.Count > 0 ? likes.Max(like => like.LikeId):1;
        like.LikeId = maxId + 1;
        likes.Add(like);
        SaveLikesAsync(likes);
        return like;
    }

    public async Task DeleteLikeAsync(Like like)
    {
        List<Like> likes = await LoadLikesAsync();
        Like likeToDelete = await GetLikeByIdAsync(like.LikeId);
        likes.Remove(likeToDelete);
        SaveLikesAsync(likes);
    }

    public async Task<Like> GetLikeByIdAsync(int id)
    {
        List<Like> likes = await LoadLikesAsync();
        Like? like = likes.SingleOrDefault(l => l.LikeId == id);
        if (like is null)
            throw new InvalidOperationException($"No like with ID {id}.");
        return like;
    }

    public IQueryable<Like> GetLikesForPost(int postId)
    {
        List<Like> likes = LoadLikesAsync().Result;
        List<Like> likesForPost =
            likes.Where(like => like.PostId == postId).ToList();
        if(likesForPost.Count==0)
            throw new InvalidOperationException("No likes for this post.");
        
        return likesForPost.AsQueryable();
    }

    public IQueryable<Like> GetAllLikes()
    {
        List<Like> likes = LoadLikesAsync().Result;
        return likes.AsQueryable();
    }
    private async Task<List<Like>> LoadLikesAsync()
    {
        string likesAsJson = await File.ReadAllTextAsync(filePath);
        List<Like> likes =
            JsonSerializer.Deserialize<List<Like>>(likesAsJson)!;
        return likes;
    }

    private async void SaveLikesAsync(List<Like> toSaveLikes)
    {
        string likesAsJson = JsonSerializer.Serialize(toSaveLikes);
        await File.WriteAllTextAsync(filePath, likesAsJson);
    }
}