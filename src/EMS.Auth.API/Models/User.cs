using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using EMS.Auth.API.Enums;
using EMS.Common.Models.BaseModel;

namespace EMS.Auth.API.Models
{
    [ExcludeFromCodeCoverage]
    public class User: BaseModel
    {
        [Column("login")]
        public string Login { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("role")]
        public RoleType Role { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<UserToken> Tokens { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not User)
            {
                return false;
            }
            User toCompare = obj as User;

            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && Login == toCompare.Login
                && Password == toCompare.Password
                && Role == toCompare.Role;
        }
    }
}
