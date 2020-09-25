using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Gateway.API.Models
{
    public class Staff: BaseModel
    {
        [Column("personId")]
        public long? PersonId { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("personId")]
        public long ManagerId { get; set; }
        [Column("manager")]
        public Person Manager { get; set; }
    }
}
