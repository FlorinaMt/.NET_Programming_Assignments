﻿using System.Text;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<List<GetCommentResponseDto>> GetCommentsAsync()
    {
        HttpResponseMessage response = await client.GetAsync("Comments");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        List<GetCommentResponseDto> receivedDto =
            JsonSerializer.Deserialize<List<GetCommentResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

        return receivedDto;
    }

    public async Task<GetCommentResponseDto> ReplaceCommentAsync(
        ReplaceCommentRequestDto request, int id)
    {
        String requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response =
            await client.PutAsync($"Comments/{id}", stringContent);
        String content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetCommentResponseDto receivedDto =
            JsonSerializer.Deserialize<GetCommentResponseDto>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }

    public async Task<IResult> DeleteCommentAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Comments/{id}");
        String content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }
        return Results.NoContent();
    }
}