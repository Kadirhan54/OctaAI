namespace OctaAI.Application.Dtos.Gemini
{
    public class TextPromptRequestDto
    {
        public string Value { get; set; }

        // Channel that haves Guid type will be used for Centrifugo channel
        public string Channel { get; set; }
    }
}
