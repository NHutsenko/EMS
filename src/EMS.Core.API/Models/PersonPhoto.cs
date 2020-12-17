using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class PersonPhoto: BaseModel
    {
        [Column("mime")]
        public string Mime { get; set; }
        [Column("base64")]
        public string Base64 { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("personId")]
        public long PersonId { get; set; }
    }
}
