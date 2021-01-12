using JWT.AuthServer.Core.Configuration;
using JWT.AuthServer.Core.DTOs;
using JWT.AuthServer.Core.Entities;
using JWT.AuthServer.Core.Repositories;
using JWT.AuthServer.Core.Services;
using JWT.AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefleshTokenService;
        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefleshTokenService = userRefreshTokenService;
        }
        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong",400,true);
            if (!(await _userManager.CheckPasswordAsync(user,loginDto.Password))) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            var token = _tokenService.CreateToken(user);
            var userRefleshToken = await _userRefleshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if(userRefleshToken == null)
            {
                await _userRefleshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Token = token.RefleshToken, Expiration = token.RefleshTokenExpiration });
            }
            else
            {
                userRefleshToken.Token = token.RefleshToken;
                userRefleshToken.Expiration = token.RefleshTokenExpiration;
            }
            await _unitOfWork.CommmitAsync();
            return Response<TokenDto>.Success(token,200);
        }

        public  Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if(client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId od ClientSecret not found",404,true);
            }

            var token = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(token,200);

        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefleshTokenService.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }
            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.Token = tokenDto.RefleshToken;
            existRefreshToken.Expiration = tokenDto.RefleshTokenExpiration;

            await _unitOfWork.CommmitAsync();

            return Response<TokenDto>.Success(tokenDto,200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefleshTokenService.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
            }

            _userRefleshTokenService.Remove(existRefreshToken);

            await _unitOfWork.CommmitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
