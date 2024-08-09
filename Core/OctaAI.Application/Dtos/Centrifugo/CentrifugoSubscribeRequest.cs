namespace OctaAI.Application.Dtos.Centrifugo
{
    public class CentrifugoSubscribeRequest
    {
        public string Channel { get; set; }

        public Guid UserId { get; set; }
    }
}
