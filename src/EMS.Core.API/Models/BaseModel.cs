using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
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
