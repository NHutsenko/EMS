using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;
using EMS.Core.API.Enums;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Contact: BaseModel
    {
        [Column("contactType")]
        public ContactType ContactType { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("personId")]
        public long PersonId { get; set; }
        [Column("value")]
        public string Value { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not Contact)
            {
                return false;
            }

            Contact toCompare = obj as Contact;

            return Id.Equals(toCompare.Id)
                && Name == toCompare.Name
                && CreatedOn == toCompare.CreatedOn
                && ContactType == toCompare.ContactType
                && Value == toCompare.Value
                && PersonId == toCompare.PersonId;
        }
    }
}
