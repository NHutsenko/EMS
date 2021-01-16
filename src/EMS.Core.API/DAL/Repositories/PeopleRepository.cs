using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.DAL.Repositories
{
    public class PeopleRepository: BaseRepository, IPeopleRepository
    {
        public PeopleRepository(IApplicationDbContext context, IDateTimeUtil dateTimeUtil) : base(context, dateTimeUtil) { }

        public IQueryable<Person> GetAll()
        {
            return _context.People.Select(e => e);
        }

        public Person GetById(long personId)
        {
            var person = _context.People
                .Include(e => e.Contacts)
                .Include(e => e.Photos)
                .Where(e => e.Id == personId)
                .Select(e => new
                {
                    e.Id,
                    e.CreatedOn,
                    e.BornedOn,
                    e.Name,
                    e.LastName,
                    e.SecondName,
                    Photos = e.Photos == null ? null : new List<PersonPhoto> { e.Photos.OrderByDescending(p => p.CreatedOn).FirstOrDefault() },
                    Contacts = e.Contacts == null ? null : e.Contacts.GroupBy(c => c.ContactType)
                        .Select(c => c.OrderByDescending(data =>data.CreatedOn)
                        .FirstOrDefault())
                        .ToList()
                })
                .FirstOrDefault();
            return new Person
            {
                Id = person.Id,
                CreatedOn = person.CreatedOn,
                BornedOn = person.BornedOn,
                Name = person.Name,
                LastName = person.LastName,
                SecondName = person.SecondName,
                Photos = person.Photos,
                Contacts = person.Contacts
            };
        }

        public async Task<int> AddAsync(Person person)
        {
            if(person is null)
            {
                throw new NullReferenceException("Person entity cannot be empty");
            }
            if (!IsNamesValid(person))
            {
                throw new ArgumentNullException("Person first name or last name cannot be empty");
            }
            if(person.BornedOn == DateTime.MinValue)
            {
                throw new ArgumentNullException("Person born date cannot be empty");
            }
            person.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.People.Add(person);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Person person)
        {
            if (person is null)
            {
                throw new NullReferenceException("Person data cannot be empty");
            }
            if (!IsNamesValid(person))
            {
                throw new ArgumentNullException("Person first name or last name cannot be empty");
            }
            if (person.BornedOn == DateTime.MinValue)
            {
                throw new ArgumentNullException("Person born date cannot be empty");
            }
            _context.People.Update(person);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddPhotoAsync(PersonPhoto photo)
        {
            if(photo is null)
            {
                throw new NullReferenceException("Photo data cannot be empty");
            }
            if (string.IsNullOrEmpty(photo.Name))
            {
                throw new ArgumentNullException("Photo name has not passed");
            }
            if(!new Regex("[^-A-Za-z0-9+/=]|=[^=]|={2,}$").IsMatch(photo.Base64))
            {
                throw new ArgumentException("Base 64 is broken");
            }

            bool parsed = new FileExtensionContentTypeProvider().TryGetContentType(photo.Name, out string mime);
            if (!parsed)
            {
                throw new ArgumentException("Unknown file MIME");
            }

            photo.Mime = mime;
            photo.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Photos.Add(photo);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddContactAsync(Contact contact)
        {
            if(contact is null)
            {
                throw new NullReferenceException("Contact data cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(contact.Value))
            {
                throw new ArgumentNullException("Contact value cannot be emty");
            }
            contact.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Contacts.Add(contact);
            return await _context.SaveChangesAsync();
        }

        private static bool IsNamesValid(Person person)
        {
            return !string.IsNullOrWhiteSpace(person.LastName) && 
                !string.IsNullOrWhiteSpace(person.Name) &&
                !string.IsNullOrEmpty(person.SecondName);
        }
    }
}
