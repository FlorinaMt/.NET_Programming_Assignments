using System.Text;
using System.Text.Json;
using ApiContracts;
using ApiContracts.LikeRelated;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<ActionResult<GetPostResponseDto>> CreatePostAsync(
        CreatePostRequestDto request)
    {
        String requestJson = JsonSerializer.Serialize(request);
        StringContent content =
            new(requestJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("Posts", content);
        String responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetPostResponseDto sendDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return sendDto;
    }

    public async Task<ActionResult<GetPostResponseDto>> GetPostAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"Posts/{id}");
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetPostResponseDto sendDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return sendDto;
    }

    public async Task<ActionResult<List<GetPostResponseDto>>> GetPostsAsync(
        string? author)
    {
        string requestUri = "Posts";
        if (!string.IsNullOrWhiteSpace(author))
            requestUri += $"?author={Uri.EscapeDataString(author.Trim())}";

        HttpResponseMessage response = await client.GetAsync(requestUri);
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        List<GetPostResponseDto> sendDto =
            JsonSerializer.Deserialize<List<GetPostResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return sendDto;
    }

    public async Task<ActionResult<GetPostResponseDto>> ReplacePostAsync(
        DeleteRequestDto request, string? title, string? body)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent content = new StringContent(requestJson, Encoding.UTF8,
            "application/json");

        HttpResponseMessage response =
            await client.PutAsync($"Posts/{request.ItemToDeleteId}", content);
        String responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetPostResponseDto sendDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return sendDto;
    }

    public async Task<IResult> DeletePostAsync(DeleteRequestDto request)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson,
            Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.SendAsync(
            new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(client.BaseAddress,
                    $"/Posts/{request.ItemToDeleteId}"),
                Content = stringContent
            });
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        return Results.NoContent();
    }

    public Task<ActionResult<GetLikeDto>> AddLikeAsync(
        AddLikeRequestDto request, int id)
    {
        throw new NotImplementedException();
        
    }

    public Task<ActionResult<GetCommentResponseDto>> AddCommentAsync(
        CreateCommentRequestDto request, int id)
    {
        throw new NotImplementedException();
    }
}