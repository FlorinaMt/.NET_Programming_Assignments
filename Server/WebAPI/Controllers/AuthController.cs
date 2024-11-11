﻿using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("auth/login")]
    public async Task<ActionResult<AddUserResponseDto>> Login([FromBody] LoginRequest request)
    {
        IQueryable<User> users = userRepository.GetUsers();
        
        User user = users.SingleOrDefault(u => u.Username == request.Username);

        if (user is null)
            return Unauthorized("Username is incorrect");
        
        if(user.Password != request.Password)
            return Unauthorized("Password is incorrect");
        
        AddUserResponseDto sendDto = new AddUserResponseDto{Username = user.Username, UserId = user.UserId};
        return sendDto;
    }
}