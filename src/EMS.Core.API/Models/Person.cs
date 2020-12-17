using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Person: BaseModel
    {
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("secondName")]
        public string SecondName { get; set; }
        [Column("bornedOn")]
        public DateTime BornedOn { get; set; }
        [IgnoreDataMember]
        public ICollection<PersonPhoto> Photos { get; set; }

        [IgnoreDataMember]
        public ICollection<Contact> Contacts { get; set; }

        [IgnoreDataMember]
        public ICollection<Staff> Staff { get; set; }
    }
}
