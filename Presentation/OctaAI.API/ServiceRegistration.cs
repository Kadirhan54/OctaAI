using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OctaAI.Application.Mappings;
using OctaAI.Domain.Identity;
using OctaAI.Persistence.Contexts.Application;
using System.Text;

namespace OctaAI.API
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<UserManager<ApplicationUser>>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddIdentity<ApplicationUser, Role>(options =>
            {
                // User Password Options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                // User Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@$";
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero, // remove delay of token when expire ??? 
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "apiWithAuthBackend",
                    ValidAudience = "apiWithAuthBackend",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eac49eb1-0a17-4a98-ba0f-f050b081d35d")),
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSecret").Value)),
                };
                options.IncludeErrorDetails = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration.GetSection("GoogleClientId").Value!;
                googleOptions.ClientSecret = configuration.GetSection("GoogleClientSecret").Value!;
            });

            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole",
                     policy => policy.RequireRole("Admin"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyHeader());
            });

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
    }
}
