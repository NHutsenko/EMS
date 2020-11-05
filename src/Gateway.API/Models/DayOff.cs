using System.ComponentModel.DataAnnotations.Schema;
using EMS.Gateway.API.Enums;

namespace EMS.Gateway.API.Models
{
    public class DayOff: BaseModel
    {
        [Column("type")]
        public DayOffType DayOffType { get; set; }
        [Column("hours")]
        public decimal Hours { get; set; }
        [Column("staffId")]
        public long StaffId { get; set; }
        [Column("Staff")]
        public Staff Staff { get; set; }
    }
}
