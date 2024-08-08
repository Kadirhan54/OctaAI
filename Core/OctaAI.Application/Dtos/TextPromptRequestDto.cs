using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Application.Dtos
{
    public class TextPromptRequestDto
    {
        public string Value { get; set; }

        // Channel that haves Guid type will be used for Centrifugo channel
        public string Channel { get; set; }
    }
}
