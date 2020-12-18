using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;
using EMS.Core.API.Enums;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class DayOff : BaseModel
    {
        [Column("type")]
        public DayOffType DayOffType { get; set; }
        [Column("hours")]
        public float Hours { get; set; }
        [Column("staffId")]
        public long StaffId { get; set; }
        [Column("Staff")]
        public Staff Staff { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not DayOff)
            {
                return false;
            }
            DayOff toCompare = obj as DayOff;

            return Id.Equals(toCompare.Id)
                   && DayOffType == toCompare.DayOffType
                   && CreatedOn == toCompare.CreatedOn
                   && StaffId == toCompare.StaffId
                   && Hours.Equals(Hours);
        }
    }
}
