using ApiContracts;
using ApiContracts.LikeRelated;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<GetPostResponseDto> CreatePostAsync (CreatePostRequestDto request);
    public Task<GetPostResponseDto> GetPostAsync(int id);
    public Task<List<GetPostResponseDto>> GetPostsAsync(string? author);
    public Task <GetPostResponseDto> ReplacePostAsync (string? title, string? body, int id);
    public Task<IResult> DeletePostAsync(int id);
    public Task <GetLikeDto> AddLikeAsync(AddLikeRequestDto request, int id);
    public Task<GetCommentResponseDto> AddCommentAsync (CreateCommentRequestDto request, int id);
}