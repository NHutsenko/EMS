using System;
using System.Linq;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not Person)
            {
                return false;
            }

            Person toCompare = obj as Person;

            return Id.Equals(toCompare.Id)
                && CreatedOn == toCompare.CreatedOn
                && Name == toCompare.Name
                && LastName == toCompare.LastName
                && SecondName == toCompare.SecondName
                && BornedOn == toCompare.BornedOn
                && Enumerable.SequenceEqual(Photos, toCompare.Photos)
                && Enumerable.SequenceEqual(Contacts, toCompare.Contacts);
        }
    }
}
