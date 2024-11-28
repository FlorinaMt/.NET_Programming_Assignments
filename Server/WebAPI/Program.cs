using System.Runtime.CompilerServices;
using EfcRepositories;
using Microsoft.OpenApi.Models;
using RepositoryContracts;
using WebAPI;
using AppContext = EfcRepositories.AppContext;

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

builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();
builder.Services.AddScoped<ILikeRepository, EfcLikeRepository>();
builder.Services.AddDbContext<AppContext>();

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

app.MapControllers();

app.Run();