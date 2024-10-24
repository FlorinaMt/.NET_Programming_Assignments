﻿using ApiContracts;
using ApiContracts.LikeRelated;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ILikeRepository likeRepository;

    public LikesController(IUserRepository userRepository,
        ILikeRepository likeRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.likeRepository = likeRepository;
        this.postRepository = postRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetLikeDto>>> GetLikesAsync()
    {
        List<GetLikeDto> likes = new List<GetLikeDto>();
        foreach (Like l in likeRepository.GetAllLikes())
        {
            likes.Add(new GetLikeDto
            {
                LikeId = l.LikeId,
                Username = (await userRepository.GetUserByIdAsync(l.UserId))
                    .Username,
                PostId = l.PostId
            });
        }

        return Ok(likes);
    }

    [HttpDelete]
    public async Task<IResult> DeleteLikeAsync(DeleteRequestDto request)
    {
        Like like = await likeRepository.GetLikeByIdAsync(request.ItemToDeleteId);

        if (like.UserId == request.UserId)
        {
            await likeRepository.DeleteLikeAsync(like.LikeId);
            return Results.NoContent();
        }

        throw new ArgumentException("No like added for this post.");
    }
}