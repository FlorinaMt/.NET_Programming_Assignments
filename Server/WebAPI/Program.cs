using FileRepositories;
using LearnWebAPI.Middlewares;
using Microsoft.OpenApi.Models;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnWebAPI", Version = "v1" });
});
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
builder.Services.AddScoped<ILikeRepository, LikeFileRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionFileRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnWebAPI v1"));
}


app.UseRouting();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseAuthorization();


app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();