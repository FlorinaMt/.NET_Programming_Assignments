using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<ActionResult<GetPostResponseDto>> GetCommentsAsync();
    public Task <ActionResult<GetCommentResponseDto>> ReplaceCommentAsync (ReplaceCommentRequestDto request);
    public Task<IResult> DeleteCommentAsync(DeleteRequestDto request);
}