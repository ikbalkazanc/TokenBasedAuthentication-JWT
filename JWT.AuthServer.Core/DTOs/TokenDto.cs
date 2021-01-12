using System;
using System.Collections.Generic;
using System.Text;

namespace JWT.AuthServer.Core.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string RefleshToken { get; set; }
        public DateTime RefleshTokenExpiration { get; set; }
    }
}
