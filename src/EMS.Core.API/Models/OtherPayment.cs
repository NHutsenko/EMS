using System.ComponentModel.DataAnnotations.Schema;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    public class OtherPayment: BaseModel
    {
        [Column("value")]
        public decimal Value { get; set; }
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
