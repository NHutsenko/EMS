using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Auth.API.Models
{
    [ExcludeFromCodeCoverage]
    public class TokenData
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not TokenData)
            {
                return false;
            }
            TokenData toCompare = obj as TokenData;
            return IsSuccess == toCompare.IsSuccess
                && ErrorMessage == toCompare.ErrorMessage
                && AccessToken == toCompare.AccessToken
                && RefreshToken == toCompare.RefreshToken
                && ExpiresIn == toCompare.ExpiresIn;
        }
    }
}
