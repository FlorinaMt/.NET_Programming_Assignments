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
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;


    public CommentsController(IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetCommentResponseDto>>>
        GetCommentsAsync()
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

    [HttpPut("{id}")]
    public async Task<ActionResult<GetCommentResponseDto>> ReplaceComment(
        [FromBody] ReplaceCommentRequestDto request, [FromRoute] int id)
    {
        //get the comment
        Comment comment =
            await commentRepository.GetCommentByIdAsync(id);


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


    [HttpDelete("{id}")]
    public async Task<IResult> DeleteCommentAsync(DeleteRequestDto request, [FromRoute] int id)
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