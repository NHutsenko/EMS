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
        [Column("user")]
        public User User { get; set; }
        [Column("accessToken", TypeName = "varchar(450)")]
        public string AccessToken { get; set; }
        [Column("refreshToken", TypeName = "varchar(450)")]
        public string RefreshToken { get; set; }
        [Column("expiresIn", TypeName = "datetime")]
        public DateTime ExpiresIn { get; set; }
        [Column("isRefreshTokenExpired")]
        public bool IsRefreshTokenExpired { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not UserToken)
            {
                return false;
            }
            UserToken toCompare = obj as UserToken;
            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && UserId == toCompare.UserId
                && AccessToken == toCompare.AccessToken
                && RefreshToken == toCompare.RefreshToken
                && ExpiresIn == toCompare.ExpiresIn
                && IsRefreshTokenExpired == toCompare.IsRefreshTokenExpired;
        }
    }
}
