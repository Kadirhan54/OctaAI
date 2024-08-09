using OctaAI.Domain.Identity;

namespace OctaAI.Domain.Entities
{
    public class UserChannel : EntityBase<Guid>
    {
        public Guid ChannelId { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
    