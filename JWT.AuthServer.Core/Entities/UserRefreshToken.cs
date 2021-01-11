using System;
using System.Collections.Generic;
using System.Text;

namespace JWT.AuthServer.Core.Entities
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
