using Centrifugo.AspNetCore.Configuration;
using Centrifugo.AspNetCore.Extensions;
using DotnetGeminiSDK;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OctaAI.API;
using OctaAI.Application.Interfaces;
using OctaAI.Domain.Identity;
using OctaAI.Infrastructure.Services;
using OctaAI.Persistence.Contexts.Application;
using OctaAI.Persistence.Services;
using Octapull.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("MSSQLConnectionString").Value);
});

builder.Services.AddWebServices(builder.Configuration);

builder.Services.AddGeminiClient(config =>
{
    config.ApiKey = "AIzaSyCXlVH6LDQcXhgQ5d1G4V48wGRb_nrN6Jo";
    config.ImageBaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-latest";
    //config.TextBaseUrl = "CURRENTLY_IMAGE_BASE_URL";
});

var centrifugoConfig = new CentrifugoOptions
{
    Url = "http://localhost:8000/api",
    ApiKey = "531794f3-4a1a-4857-ba5b-2483ec24faba", 
};

builder.Services.AddCentrifugoClient(centrifugoConfig);
builder.Services.AddScoped<ICentrifugoService, CentrifugoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
