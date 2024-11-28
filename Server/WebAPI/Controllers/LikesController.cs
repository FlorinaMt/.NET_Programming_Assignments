using ApiContracts;
using ApiContracts.LikeRelated;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly ILikeRepository likeRepository;

    public LikesController(IUserRepository userRepository,
        ILikeRepository likeRepository)
    {
        this.userRepository = userRepository;
        this.likeRepository = likeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetLikeDto>>> GetLikesAsync()
    {
        List<GetLikeDto> likes = new List<GetLikeDto>();
        foreach (Like l in await likeRepository.GetAllLikes().ToListAsync())
        {
            likes.Add(new GetLikeDto
            {
                LikeId = l.LikeId,
                Username =
                    (await userRepository.GetUserByIdAsync(l.User.UserId))
                    .Username,
                PostId = l.Post.PostId
            });
        }

        return Ok(likes);
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteLikeAsync([FromRoute] int id)
    {
        Like like = await likeRepository.GetLikeByIdAsync(id);

        await likeRepository.DeleteLikeAsync(like.LikeId);
        return Results.NoContent();
    }
}