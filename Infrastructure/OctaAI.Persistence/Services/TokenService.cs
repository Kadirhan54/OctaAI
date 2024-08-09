using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OctaAI.Application.Interfaces;
using OctaAI.Domain.Identity;
using OctaAI.Persistence.Contexts.Application;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Octapull.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private const int ExpirationMinutes = 30;
        private readonly ApplicationDbContext applicationDbContext;

        public TokenService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public string CreateToken(ApplicationUser user)
        {

            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",
                "apiWithAuthBackend",
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(ApplicationUser user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub,  user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    //new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(ClaimTypes.Email, user.Email),
                };

                //var userWithChannels = applicationDbContext.Users
                //    .Where(u => u.Id == user.Id)
                //    .Include(u => u.Channels)
                //    .FirstOrDefault();


                //if (userWithChannels?.Channels != null && userWithChannels.Channels.Any())
                //{
                //    // Get channel names as a list of strings
                //    var channelNames = userWithChannels.Channels.Select(c => c.ChannelId).ToList();

                //    // Serialize the list of channel names to JSON
                //    var channelNamesJson = JsonConvert.SerializeObject(channelNames);

                //    // Add the JSON string as a claim
                //    claims.Add(new Claim("channels", channelNamesJson));
                //}
                //else
                //{
                //    // Add an empty JSON array as a claim if there are no channels
                //    claims.Add(new Claim("channels", "[]"));
                //}


                //if (userWithChannels?.Channels != null && userWithChannels.Channels.Any())
                //{
                //    // Concatenate all channel names into a single string, separated by commas
                //    var channelNames = string.Join(",", userWithChannels.Channels.Select(c => c.ChannelId));

                //    // Add the concatenated string as a single claim
                //    claims.Add(new Claim("channels", channelNames));
                //}
                //else
                //{
                //    claims.Add(new("channels", ""));
                //}

                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("eac49eb1-0a17-4a98-ba0f-f050b081d35d")
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
