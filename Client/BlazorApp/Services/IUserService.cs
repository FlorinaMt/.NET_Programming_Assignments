using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<AddUserResponseDto> AddUserAsync(AddUserRequestDto request);
    public Task<ActionResult<AddUserResponseDto>> GetUserAsync(int id);
    public Task<List<string>> GetAllUsersAsync(string? nameContains);
    public Task <ActionResult<AddUserResponseDto>> ReplaceUserAsync(ReplaceUserRequestDto request);
    public Task<IResult> DeleteUserAsync(DeleteUserRequestDto request);
}