namespace OctaAI.Domain.Common
{
    public interface ICreatedByEntity
    {
        public Guid? CreatedByUserId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
