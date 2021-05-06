using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EMS.Auth.API.Services
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public AuthService(IUsersRepository usersRepository,
            ITokenRepository tokenRepository,
            IDateTimeUtil dateTimeUtil,
            IEMSLogger<AuthService> logger,
            JwtSecurityTokenHandler jwtSecurityTokenHandler) : base(logger, dateTimeUtil)
        {
            _tokenRepository = tokenRepository;
            _usersRepository = usersRepository;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public virtual async Task<TokenData> AuthUserAsync(LoginUserRequest request)
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
                if (result == 0)
                {
                    throw new Exception("An error occured while saving token data");
                }
                TokenData tokenResponse = new()
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
                return new TokenData
                {
                    IsSuccess = false,
                    ErrorMessage = aex.Message,
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(AuthUserAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new TokenData
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occured while authentcating user {request.Login}",
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(AuthUserAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return new TokenData
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occured while authentcating user {request.Login}",
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty
                };
            }
        }

        public virtual async Task<TokenData> RefreshTokenAsync(TokenData toRefresh)
        {
            try
            {
                UserToken userToken = _tokenRepository.GetTokenData(toRefresh.AccessToken);
                if (userToken is null)
                {
                    throw new ArgumentException($"Token data for token {toRefresh.AccessToken} does mot exists into DB");
                }
                User user = _usersRepository.GetById(userToken.UserId);
                DateTime creationDate = _dateTimeUtil.GetCurrentDateTime();
                JwtSecurityToken access = GetToken(user, creationDate, TokenType.Access);
                JwtSecurityToken refresh = GetToken(user, creationDate, TokenType.Refresh);
                UserToken newToken = new()
                {
                    AccessToken = _jwtSecurityTokenHandler.WriteToken(access),
                    RefreshToken = _jwtSecurityTokenHandler.WriteToken(refresh),
                    ExpiresIn = creationDate.AddMinutes(5),
                    UserId = user.Id,
                    IsRefreshTokenExpired = false
                };
                int result = await _tokenRepository.SaveTokenAsync(newToken);
                if (result == 0)
                {
                    throw new Exception("An error occured while saving new access token");
                }
                await _tokenRepository.DisableRefreshTokenAsync(toRefresh.RefreshToken);
                TokenData response = new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty,
                    AccessToken = _jwtSecurityTokenHandler.WriteToken(access),
                    RefreshToken = _jwtSecurityTokenHandler.WriteToken(refresh),
                    ExpiresIn = creationDate.AddMinutes(5)
                };
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(RefreshTokenAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = toRefresh,
                    Response = response
                };
                _logger.AddLog(logData);
                return response;
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(AuthService),
                    CallerMethodName = nameof(RefreshTokenAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = toRefresh,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return new TokenData
                {
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty,
                    IsSuccess = false,
                    ErrorMessage = "Authentication failed"
                };
            }
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
