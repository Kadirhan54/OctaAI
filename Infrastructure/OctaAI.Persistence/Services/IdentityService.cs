
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OctaAI.Application.Dtos.Account.Request;
using OctaAI.Application.Interfaces;
using OctaAI.Application.Models;
using OctaAI.Domain.Identity;
using OctaAI.Persistence.Utils;

namespace OctaAI.Persistence.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
        }

        //public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal)
        //{
        //    var user = await _userManager.GetUserAsync(claimsPrincipal);

        //    return user;
        //}

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user?.UserName;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }
        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(RegisterRequestDto registerRequestDto)
        {
            var userId = Guid.NewGuid();

            //var user = _mapper.Map<ApplicationUser>(createUserRequestDto);

            var user = new ApplicationUser()
            {
                Id = userId,
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.SurName,
                PhoneNumber = registerRequestDto.PhoneNumber,
                BirthDate = registerRequestDto.BirthDate.ToUniversalTime(),
                CreatedOn = DateTimeOffset.UtcNow,
                CreatedByUserId = userId,
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            return (result.ToApplicationResult(), user.Id.ToString());
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<bool> SignInUser(ApplicationUser user, string password)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            return loginResult.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}
