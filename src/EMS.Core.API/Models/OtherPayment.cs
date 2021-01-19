using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class OtherPayment: BaseModel
    {
        [Column("value")]
        public double Value { get; set; }
        [Column("personId")]
        public long PersonId { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("comment")]
        public string Comment { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not OtherPayment)
            {
                return false;
            }

            OtherPayment toCompare = obj as OtherPayment;

            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && Value == toCompare.Value
                && PersonId == toCompare.PersonId
                && Comment == toCompare.Comment;
        }
    }
}
