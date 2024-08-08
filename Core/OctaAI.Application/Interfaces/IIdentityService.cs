
using OctaAI.Application.Dtos.Account.Request;
using OctaAI.Application.Models;
using OctaAI.Domain.Identity;

namespace OctaAI.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        //Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal);

        Task<ApplicationUser?> GetUserByIdAsync(string userId);

        Task<ApplicationUser?> GetUserByUserNameAsync(string userName);

        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<bool> SignInUser(ApplicationUser user, string password);

        Task<(Result Result, string UserId)> CreateUserAsync(RegisterRequestDto createUserRequestDto);

        Task<Result> DeleteUserAsync(string userId);
    }
}
