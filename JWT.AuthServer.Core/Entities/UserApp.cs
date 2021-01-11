using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWT.AuthServer.Core.Entities
{
    public class UserApp : IdentityUser
    {
        public string City { get; set; }
    }
}
