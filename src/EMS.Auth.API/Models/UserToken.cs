using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Auth.API.Models
{
    [ExcludeFromCodeCoverage]
    public class UserToken: BaseModel
    {
        [Column("userId")]
        public long UserId { get; set; }
        [Column("accessToken")]
        public string AccessToken { get; set; }
        [Column("refreshToken")]
        public string RefreshToken { get; set; }
        [Column("expiresIn")]
        public DateTime ExpiresIn { get; set; }
        [Column("isRefreshTokenExpired")]
        public bool IsRefreshTokenExpired { get; set; }
    }
}
