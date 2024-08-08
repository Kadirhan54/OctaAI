using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Domain.Entities
{
    public class AiResponse : EntityBase<Guid>
    {
        public string Channel { get; set; }

        public string Message { get; set; }

        public Guid UserId { get; set; }
    }
}
