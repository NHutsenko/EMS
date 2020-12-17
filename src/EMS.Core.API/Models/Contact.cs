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
    }
}
