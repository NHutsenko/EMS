using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class PersonPhoto : BaseModel
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("mime")]
        public string Mime { get; set; }
        [Column("base64")]
        public string Base64 { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("personId")]
        public long PersonId { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not PersonPhoto)
            {
                return false;
            }

            PersonPhoto toCompare = obj as PersonPhoto;

            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && Name == toCompare.Name
                && Mime == toCompare.Mime
                && Base64 == toCompare.Base64
                && PersonId == toCompare.PersonId;

        }
    }
}
