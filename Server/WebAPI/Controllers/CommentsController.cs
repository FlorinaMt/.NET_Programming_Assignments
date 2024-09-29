using ApiContracts;
using ApiContracts.LikeRelated;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ILikeRepository likeRepository;
    private readonly ICommentRepository commentRepository;


    public CommentsController(IPostRepository postRepository,
        IUserRepository userRepository, ILikeRepository likeRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.likeRepository = likeRepository;
        this.commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetCommentResponseDto>>>
        GetCommentAsync()
    {
        List<GetCommentResponseDto>
            comments = new List<GetCommentResponseDto>();
        foreach (Comment c in commentRepository.GetAllComments())
        {
            comments.Add(new GetCommentResponseDto
            {
                CommentId = c.CommentId, Body = c.CommentBody,
                AuthorUsername =
                    (await userRepository.GetUserByIdAsync(c.UserId)).Username,
                PostId = c.PostId
            });
        }

        return Ok(comments);
    }

    [HttpPut]
    public async Task<ActionResult<GetCommentResponseDto>> ReplaceComment(
        [FromBody] ReplaceCommentRequestDto request, [FromQuery] string? body)
    {
        //get the comment
        Comment comment =
            await commentRepository.GetCommentByIdAsync(request.CommentId);


        //if this is the author, they can update it
        if (comment.UserId == request.UserId)
        {
            Comment newComment = new Comment
            {
                CommentBody = request.Body, CommentId = comment.CommentId,
                PostId = comment.PostId, UserId = comment.UserId
            };
            
            await commentRepository.UpdateCommentAsync(newComment);

            GetCommentResponseDto updatedComment = new GetCommentResponseDto
            {
                CommentId = newComment.CommentId, Body = newComment.CommentBody,
                AuthorUsername =
                    (await userRepository.GetUserByIdAsync(newComment.UserId))
                    .Username, PostId = newComment.PostId
            };
            
            return Ok(updatedComment);
        }

        throw new ArgumentException("Only the author can update this comment.");
    }


    [HttpDelete]
    public async Task<IResult> DeleteLikeAsync(DeleteRequestDto request)
    {
        Comment comment =
            await commentRepository.GetCommentByIdAsync(request.ItemToDeleteId);

        if (comment.UserId == request.UserId)
        {
            await commentRepository.DeleteCommentAsync(comment.CommentId);
            return Results.NoContent();
        }

        throw new ArgumentException("No comment added for this post.");
    }
}