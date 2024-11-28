using ApiContracts;
using ApiContracts.LikeRelated;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        foreach (Comment c in await commentRepository.GetAllComments()
                     .ToListAsync())
        {
            comments.Add(new GetCommentResponseDto
            {
                CommentId = c.CommentId, Body = c.CommentBody,
                AuthorUsername = c.User.Username,
                PostId = c.Post.PostId
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
        if (comment.User.UserId == request.UserId)
        {
            Comment newComment = Comment.getComment();
            newComment.CommentBody = request.Body;
            newComment.CommentId = comment.CommentId;
            newComment.Post = comment.Post;
            newComment.User = comment.User;

            await commentRepository.UpdateCommentAsync(newComment);

            GetCommentResponseDto updatedComment = new GetCommentResponseDto
            {
                CommentId = newComment.CommentId, Body = newComment.CommentBody,
                AuthorUsername = newComment.User.Username,
                PostId = newComment.Post.PostId
            };

            return Ok(updatedComment);
        }

        throw new ArgumentException("Only the author can update this comment.");
    }


    [HttpDelete("{id}")]
    public async Task<IResult> DeleteCommentAsync([FromRoute] int id)
    {
        Comment comment =
            await commentRepository.GetCommentByIdAsync(id);

        await commentRepository.DeleteCommentAsync(comment.CommentId);
        return Results.NoContent();
    }
}