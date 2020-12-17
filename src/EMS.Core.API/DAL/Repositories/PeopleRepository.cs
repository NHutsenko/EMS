using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
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
            Person person = _context.People
                .Include(e => e.Contacts)
                .Include(e => e.Photos.OrderByDescending(e => e.CreatedOn)
                    .FirstOrDefault())
                .FirstOrDefault(e => e.Id == personId);
            person.Contacts = person.Contacts.GroupBy(e => e.ContactType)
                .Select(e =>
                    e.OrderByDescending(c => c.CreatedOn)
                    .FirstOrDefault())
                .ToList();
            return person;
        }

        public async Task<int> AddAsync(Person person)
        {
            if(person is null)
            {
                throw new ArgumentException("Person entity cannot be empty");
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
                throw new ArgumentNullException("Person data cannot be empty");
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
                throw new ArgumentNullException("Photo data cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(photo.Mime))
            {
                throw new ArgumentNullException("Mime type has not passed");
            }
            if(!new Regex("[^-A-Za-z0-9+/=]|=[^=]|={3,}$").IsMatch(photo.Base64))
            {
                throw new ArgumentException("Base 64 is broken");
            }
            photo.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Photos.Add(photo);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddContactAsync(Contact contact)
        {
            if(contact is null)
            {
                throw new ArgumentNullException("Contact data cannot be empty");
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
                !string.IsNullOrWhiteSpace(person.Name);
        }
    }
}
