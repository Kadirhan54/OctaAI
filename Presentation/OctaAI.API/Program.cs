using Centrifugo.AspNetCore.Configuration;
using Centrifugo.AspNetCore.Extensions;
using DotnetGeminiSDK;
using OctaAI.Domain.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder => builder.WithOrigins("http://localhost:4200")
//                          .AllowAnyHeader()
//                          .AllowAnyMethod());
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddGeminiClient(config =>
{
    config.ApiKey = "AIzaSyAX7SA7Nm1iWKNK7HL1buzdWegL7jMR204";
    config.ImageBaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-latest";
    //config.TextBaseUrl = "CURRENTLY_IMAGE_BASE_URL";
});

var centrifugoConfig = new CentrifugoOptions
{
    Url = "http://localhost:8000/api",
    ApiKey = "531794f3-4a1a-4857-ba5b-2483ec24faba", 

};

builder.Services.AddCentrifugoClient(centrifugoConfig);

//builder.Services.AddIdentity<ApplicationUser, Role>(options =>
//{
//    // User Password Options
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 6;
//    options.Password.RequiredUniqueChars = 0;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    // User Username and Email Options
//    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@$";
//    options.User.RequireUniqueEmail = true;
//}).AddEntityFrameworkStores<ApplicationDbContext>();

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
