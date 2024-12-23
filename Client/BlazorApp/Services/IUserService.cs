﻿using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<AddUserResponseDto> AddUserAsync(AddUserRequestDto request);
    public Task<AddUserResponseDto> GetUserAsync(int id);
    public Task<List<string>> GetAllUsersAsync(string? nameContains);
    public Task <AddUserResponseDto> ReplaceUserAsync(ReplaceUserRequestDto request, int id);
    public Task<IResult> DeleteUserAsync(DeleteUserRequestDto reques, int id);
}