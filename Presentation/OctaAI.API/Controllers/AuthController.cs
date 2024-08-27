using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctaAI.Application.Dtos.Account.Request;
using OctaAI.Application.Dtos.Account.Response;
using OctaAI.Application.Interfaces;
using OctaAI.Persistence.Contexts.Application;
using System.Security.Claims;
using OctaAI.Domain.Identity;


namespace OctaAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IIdentityService _identityService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICentrifugoService _centrifugoService;

        public AuthController(ITokenService tokenService, IIdentityService userService, ApplicationDbContext applicationDbContext, ICentrifugoService centrifugoService)
        {
            _tokenService = tokenService;
            _identityService = userService;
            _applicationDbContext = applicationDbContext;
            _centrifugoService = centrifugoService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromForm] LoginRequestDto loginRequestDto)
        {
            var user = await _identityService.GetUserByEmailAsync(loginRequestDto.Email);
            if (user == null) return Unauthorized("Invalid email");

            var result = await _identityService.SignInUser(user, loginRequestDto.Password);
            if (!result) return Unauthorized("Invalid password");

            var userWithChannels = _applicationDbContext.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.Channels)
                .FirstOrDefault();

            foreach (var channel in userWithChannels.Channels)
            {
                _centrifugoService.Subscribe(userWithChannels.Id.ToString(), channel.ChannelId.ToString());
            }

            return new LoginResponseDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromForm] RegisterRequestDto registerDto)
        {
            var isUserNull = await _identityService.GetUserByEmailAsync(registerDto.Email);

            if (isUserNull != null)
            {
                return BadRequest("User already exists");
            }

            var (result, userId) = await _identityService.CreateUserAsync(registerDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            //// Generate email confirmation token
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //// Construct confirmation link
            //var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);

            //// Send confirmation email
            //await _emailService.SendConfirmationEmailAsync(model.Email, confirmationLink);

            //return Ok("User registered successfully. Please check your email for confirmation.");

            var user = await _identityService.GetUserByIdAsync(userId);

            var token = _tokenService.CreateToken(user);

            return new RegisterResponseDto
            {
                Email = user.Email,
                Token = token
            };

        }

        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest(); // Handle failure scenario

            // Find or create Identity user
            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _identityService.GetUserByEmailAsync(email);

            if (user == null)
            {
                var name = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
                var surname = authenticateResult.Principal.FindFirst(ClaimTypes.Surname)?.Value;

                user = new ApplicationUser { UserName = email, Email = email,Id=Guid.NewGuid(), FirstName= name ,LastName= surname };
                await _identityService.CreateUserAsync(user);
            }

            // Generate JWT token here
            var token = _tokenService.CreateToken(user);

            // Redirect to Angular with the token as a query parameter
            string redirectUrl = $"http://localhost:4200/login?token={token}";
            return Redirect(redirectUrl);
        }
    }
}
