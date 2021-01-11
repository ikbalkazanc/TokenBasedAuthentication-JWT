using JWT.AuthServer.Core.Configuration;
using JWT.AuthServer.Core.DTOs;
using JWT.AuthServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWT.AuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);

        ClientTokenDto CreateTokenByClient(Client client);
    }
}
