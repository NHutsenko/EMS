using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class MotivationModificator: BaseModel
    {
        [Column("modValue")]
        public double ModValue { get; set; }
        [Column("staff")]
        public Staff Staff { get; set; }
        [Column("StaffId")]
        public long StaffId { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not MotivationModificator)
            {
                return false;
            }

            MotivationModificator toCompare = obj as MotivationModificator;

            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && ModValue == toCompare.ModValue
                && StaffId == toCompare.StaffId;
        }
    }
}
