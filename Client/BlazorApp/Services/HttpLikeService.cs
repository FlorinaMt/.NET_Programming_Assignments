using System.Text;
using System.Text.Json;
using ApiContracts;
using ApiContracts.LikeRelated;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpLikeService : ILikeService
{
    private readonly HttpClient client;

    public HttpLikeService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<ActionResult<List<GetLikeDto>>> GetLikesAsync()
    {
        HttpResponseMessage response = await client.GetAsync("Likes");
        string content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        List<GetLikeDto> receivedDto =
            JsonSerializer.Deserialize<List<GetLikeDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

        return receivedDto;
    }

    public async Task<IResult> DeleteLikeAsync(DeleteRequestDto request)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent requestContent = new StringContent(requestJson,
            Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.SendAsync(
            new HttpRequestMessage
            {
                Method = HttpMethod.Delete, Content = requestContent,
                RequestUri = new Uri("Likes")
            });
        string content = await response.Content.ReadAsStringAsync();

        return Results.NoContent();
    }
}