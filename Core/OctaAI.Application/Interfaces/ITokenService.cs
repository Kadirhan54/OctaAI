using OctaAI.Domain.Identity;

namespace OctaAI.Application.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUser user);
    }
}
