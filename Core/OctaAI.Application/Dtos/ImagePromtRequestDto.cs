using Microsoft.AspNetCore.Http;

namespace OctaAI.Application.Dtos
{
    public class ImagePromtRequestDto
    {
        public IFormFile File { get; set; }
        public string Prompt { get; set; }  
    }
}
