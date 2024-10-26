﻿using ApiContracts;
using ApiContracts.LikeRelated;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface ILikeService
{
    public Task<ActionResult<GetLikeDto>> GetLikesAsync();
    public Task<IResult> DeleteLikeAsync(DeleteRequestDto request);
}