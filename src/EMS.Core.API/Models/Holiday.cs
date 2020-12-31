using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Holiday: BaseModel
    {
        [Column("holidayDate")]
        public DateTime HolidayDate { get; set; }
        [Column("toDoDay")]
        public DateTime? ToDoDate { get; set; }
        [Column("Description")]
        public string Description { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not Holiday)
            {
                return false;
            }
            Holiday toCompare = obj as Holiday;

            return Id == toCompare.Id
                && CreatedOn == toCompare.CreatedOn
                && HolidayDate == toCompare.HolidayDate
                && ToDoDate == toCompare.ToDoDate
                && Description == toCompare.Description;
        }
    }
}
