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

    public async Task<GetPostResponseDto> CreatePostAsync(
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

        GetPostResponseDto receivedDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }

    public async Task<GetPostResponseDto> GetPostAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"Posts/{id}");
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetPostResponseDto receivedDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }

    public async Task<List<GetPostResponseDto>> GetPostsAsync(
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

        List<GetPostResponseDto> receivedDto =
            JsonSerializer.Deserialize<List<GetPostResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }

    public async Task<GetPostResponseDto> ReplacePostAsync(string? title,
        string? body, int id)
    {
        // Construct the query parameters string
        var queryParameters = new List<string>();
        if (!string.IsNullOrWhiteSpace(title))
            queryParameters.Add($"title={Uri.EscapeDataString(title)}");
        if (!string.IsNullOrWhiteSpace(body))
            queryParameters.Add($"body={Uri.EscapeDataString(body)}");
        string queryString = string.Join("&", queryParameters);

        string requestUrl = $"Posts/{id}";
        if (!string.IsNullOrWhiteSpace(queryString))
            requestUrl += $"?{queryString}";

        HttpResponseMessage response = await client.PutAsync(requestUrl, null);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(
                $"Error: {response.StatusCode}, {responseContent}");
            throw new Exception(
                $"Error: {response.StatusCode}, {responseContent}");
        }

        // Deserialize and return the response
        GetPostResponseDto receivedDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            )!;

        return receivedDto;
    }
    

    public async Task<IResult> DeletePostAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"/Posts/{id}");
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        return Results.NoContent();
    }

    public async Task<GetLikeDto> AddLikeAsync(
        AddLikeRequestDto request, int id)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson,
            Encoding.UTF8, "application/json");

        HttpResponseMessage response =
            await client.PostAsync($"Posts/{id}/Likes", stringContent);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"{response.StatusCode}, {content}");
        }

        GetLikeDto receivedDto = JsonSerializer.Deserialize<GetLikeDto>(content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return receivedDto;
    }

    public async Task<GetCommentResponseDto> AddCommentAsync(
        CreateCommentRequestDto request, int id)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson,
            Encoding.UTF8, "application/json");

        HttpResponseMessage response =
            await client.PostAsync($"Posts/{id}/Comments", stringContent);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
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
}