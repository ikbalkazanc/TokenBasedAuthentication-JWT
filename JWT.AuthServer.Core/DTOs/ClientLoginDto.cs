using System;
using System.Collections.Generic;
using System.Text;

namespace JWT.AuthServer.Core.DTOs
{
    public class ClientLoginDto
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
