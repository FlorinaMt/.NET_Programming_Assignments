using ApiContracts;
using ApiContracts.LikeRelated;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<ActionResult<GetPostResponseDto>> CreatePostAsync (CreatePostRequestDto request);
    public Task<ActionResult<GetPostResponseDto>> GetPostAsync(int id);
    public Task<ActionResult<List<GetPostResponseDto>>> GetPostsAsync(string? author);
    public Task <ActionResult<GetPostResponseDto>> ReplacePostAsync (DeleteRequestDto request, string? title, string? body, int id);
    public Task<IResult> DeletePostAsync(DeleteRequestDto request, int id);
    public Task<ActionResult<GetLikeDto>> AddLikeAsync(AddLikeRequestDto request, int id);
    public Task<ActionResult<GetCommentResponseDto>> AddCommentAsync (CreateCommentRequestDto request, int id);
}