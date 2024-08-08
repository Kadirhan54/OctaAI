using Microsoft.AspNetCore.Http;

namespace OctaAI.Application.Dtos.Account.Request
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
    }
}
