using Microsoft.AspNetCore.Http;

namespace OctaAI.Application.Dtos.Gemini
{
    public class ImagePromtRequestDto
    {
        public IFormFile File { get; set; }
        public string Prompt { get; set; }
    }
}
