using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<ActionResult<List<GetCommentResponseDto>>> GetCommentsAsync();
    public Task <ActionResult<GetCommentResponseDto>> ReplaceCommentAsync (ReplaceCommentRequestDto request, int id);
    public Task<IResult> DeleteCommentAsync(DeleteRequestDto request, int id);
}