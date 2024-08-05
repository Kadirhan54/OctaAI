using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Domain.Common
{
    internal interface IEntityBase<T>
    {
        public T Id { get; set; }
    }
}
