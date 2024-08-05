using OctaAI.Domain.Entities;

namespace OctaAI.Domain.Identity
{
    public class UserSetting : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Int16 TimeZone { get; set; }
        public string LanguageCode { get; set; }
    }
}
