using OctaAI.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Application.Interfaces
{
    internal interface ITokenService
    {
        public string CreateToken(ApplicationUser user);
    }
}
