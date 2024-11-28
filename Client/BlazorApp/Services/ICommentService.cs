using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<List<GetCommentResponseDto>> GetCommentsAsync();
    public Task <GetCommentResponseDto> ReplaceCommentAsync (ReplaceCommentRequestDto request, int id);
    public Task<IResult> DeleteCommentAsync(int id);
}