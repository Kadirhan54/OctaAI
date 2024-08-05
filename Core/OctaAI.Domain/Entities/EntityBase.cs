using OctaAI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Domain.Entities
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>, ICreatedByEntity, IModifiedByEntity, IDeletedByEntity
    {
        public virtual TKey Id { get; set; }

        public Guid? CreatedByUserId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
        public string? ModifiedByUserId { get; set; }

        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
