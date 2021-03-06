﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Staff: BaseModel
    {
        [Column("personId")]
        public long? PersonId { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("managerId")]
        public long ManagerId { get; set; }
        [Column("manager")]
        public Person Manager { get; set; }
        [Column("positionId")]
        public long PositionId { get; set; }
        [Column("position")]
        public Position Position { get; set; }
        [Column("motivationMod")]
        public MotivationModificator MotivationModificator { get; set; }
        [Column("motivationModId")]
        public long? MotivationModificatorId { get; set; }
        [Column("roadMap")]
        public RoadMap RoadMap { get; set; }
        [Column("roadMapId")]
        public long RoadMapId { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not Staff)
            {
                return false;
            }
            Staff toCompare = obj as Staff;

            return Id.Equals(toCompare.Id)
                   && PersonId == toCompare.PersonId
                   && ManagerId == toCompare.ManagerId
                   && PositionId == toCompare.PositionId
                   && CreatedOn == toCompare.CreatedOn;
        }
    }
}
