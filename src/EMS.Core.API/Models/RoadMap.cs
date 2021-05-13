using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;
using EMS.Core.API.Enums;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class RoadMap: BaseModel
    {
        [Column("status")]
        public RoadMapStatus Status { get; set; }
        [Column("tasks")]
        public string Tasks { get; set; }
        [Column("staff")]
        public Staff Staff { get; set; }
        [Column("staffId")]
        public long StaffId { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not RoadMap)
            {
                return false;
            }
            RoadMap toCompare = obj as RoadMap;

            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && Status == toCompare.Status
                && Tasks == toCompare.Tasks
                && StaffId == toCompare.StaffId;
        }
    }
}
