using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace EMS.Auth.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IDateTimeUtil _dateTimeUtil;
        private readonly IEMSLogger<AuthService> _logger;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public AuthService(IUsersRepository usersRepository, 
            ITokenRepository tokenRepository, 
            IDateTimeUtil dateTimeUtil, 
            IEMSLogger<AuthService> logger,
            JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _tokenRepository = tokenRepository;
            _usersRepository = usersRepository;
            _dateTimeUtil = dateTimeUtil;
            _logger = logger;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public async Task<TokenResponse> AuthUserAsync(LoginUserRequest request)
        {
            try
            {
                User user = _usersRepository.VerifyUser(request.Login, request.Password);
                if (user is null)
                {
                    throw new ArgumentException("Invalid login or password");
                }
                DateTime creationDate = _dateTimeUtil.GetCurrentDateTime();
                JwtSecurityToken accessToken = GetToken(user, creationDate, TokenType.Access);
                JwtSecurityToken refreshToken = GetToken(user, creationDate, TokenType.Refresh);
                UserToken userToken = new()
                {
                    UserId = user.Id,
                    ExpiresIn = creationDate.AddMinutes(AuthOptions.LifeTime),
                    AccessToken = _jwtSecurityTokenHandler.WriteToken(accessToken),
                    RefreshToken = _jwtSecurityTokenHandler.WriteToken(refreshToken),
                    IsRefreshTokenExpired = false
                };
                int result = await _tokenRepository.SaveTokenAsync(userToken);
                if(result == 0)
                {
                    throw new InvalidOperationException("An error occured while saving token data");
                }
                TokenResponse tokenResponse = new()
                {
                    AccessToken = userToken.AccessToken,
                    RefreshToken = userToken.RefreshToken,
                    ExpiresIn = userToken.ExpiresIn,
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(AuthUserAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = tokenResponse
                };
                _logger.AddLog(logData);
                return tokenResponse;
            }
            catch (ArgumentException aex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(AuthUserAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = aex
                };
                _logger.AddErrorLog(logData);
                return new TokenResponse
                {
                    IsSuccess = false,
                    ErrorMessage = aex.Message
                };
            }
            catch(InvalidOperationException ioex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(AuthUserAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ioex
                };
                _logger.AddErrorLog(logData);
                return new TokenResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occured while authentcating user {request.Login}"
                };
            }

        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        private static JwtSecurityToken GetToken(User user, DateTime creationDate, TokenType tokenType)
        {
            ClaimsIdentity claimsIdentity = GetClaims(user);

            JwtSecurityToken jwt = new(issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: creationDate,
                expires: creationDate.AddMinutes(AuthOptions.LifeTime),
                claims: claimsIdentity.Claims,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(tokenType), SecurityAlgorithms.HmacSha256));
            return jwt;
        }

        private static ClaimsIdentity GetClaims(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim(ClaimsIdentity.DefaultIssuer, AuthOptions.Issuer)
            };
            ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
