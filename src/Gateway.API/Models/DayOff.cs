using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Gateway.API.Enums;

namespace EMS.Gateway.API.Models
{
    [ExcludeFromCodeCoverage]
    public class DayOff: BaseModel
    {
        [Column("type")]
        public DayOffType DayOffType { get; set; }
        [Column("hours")]
        public float Hours { get; set; }
        [Column("staffId")]
        public long StaffId { get; set; }
        [Column("Staff")]
        public Staff Staff { get; set; }
    }
}
