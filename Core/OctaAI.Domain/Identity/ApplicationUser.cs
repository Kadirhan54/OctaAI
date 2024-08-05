using Microsoft.AspNetCore.Identity;
using OctaAI.Domain.Common;
using OctaAI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IEntityBase<Guid>, ICreatedByEntity, IModifiedByEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public Gender Gender { get; set; }
        //public string ImageId { get; set; }

        public UserSetting UserSetting { get; set; }

        //public ICollection<Meeting> Meetings { get; set; }

        public Guid? CreatedByUserId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}
