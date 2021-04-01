using System.Linq;
using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Auth.API.DAL.Repositories
{
    public class TokenRepository: BaseRepository, ITokenRepository
    {
        public TokenRepository(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil) : base(applicationDbContext, dateTimeUtil) { }

        public virtual async Task<int> DisableRefreshTokenAsync(string refreshToken)
        {
            UserToken token = _context.Tokens.FirstOrDefault(e => e.RefreshToken == refreshToken);
            if(token is null)
            {
                return 0;
            }
            token.IsRefreshTokenExpired = true;
            _context.Tokens.Update(token);
            return await _context.SaveChangesAsync();
        }

        public virtual UserToken GetTokenData(string accessToken)
        {
            return _context.Tokens.FirstOrDefault(e => e.AccessToken == accessToken);
        }

        public virtual async Task<int> SaveTokenAsync(UserToken userToken)
        {
            userToken.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Tokens.Add(userToken);
            return await _context.SaveChangesAsync();
        }
    }
}
