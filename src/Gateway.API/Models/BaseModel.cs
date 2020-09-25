using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Gateway.API.Models
{
    public class BaseModel
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column("createdOn")]
        public DateTime CreatedOn { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
