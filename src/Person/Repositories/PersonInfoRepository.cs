using EMS.Person.Context;
using EMS.Person.Interfaces;
using EMS.Person.Models;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Repositories;

public sealed class InfoRepository: RepositoryBase<PersonInfo>, IInfoRepository
{
    public InfoRepository(PersonContext dbContext) : base(dbContext)
    {
    }


    public async Task<int> AddPersonInfoAsync(string lastName, string firstName, string login, string about, bool gender, DateTime bornOn, CancellationToken cancellationToken)
    {
        if (await DbContext.People.AnyAsync(e => e.Login == login, cancellationToken: cancellationToken))
        {
            throw new AlreadyExistsException($"Person with login '{login}' already exists");
        }

        PersonInfo info = new()
        {
            LastName = lastName,
            FirstName = firstName,
            BornOn = bornOn,
            Gender = gender,
            Login = login,
            About = about
        };
        
        await DbContext.People.AddAsync(info, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return info.Id;
    }

    public async Task UpdateNamesAsync(int id, string firstName, string lastName, CancellationToken cancellationToken)
    {
        PersonInfo info = await GetPersonAsync(id, cancellationToken);

        if (string.IsNullOrWhiteSpace(firstName) is false)
        {
            DbContext.Entry(info).Property(e => e.FirstName).CurrentValue = firstName;
        }

        if (string.IsNullOrWhiteSpace(lastName) is false)
        {
            DbContext.Entry(info).Property(e => e.LastName).CurrentValue = lastName;
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateGenderAsync(int id, bool gender, CancellationToken cancellationToken)
    {
        PersonInfo info = await GetPersonAsync(id, cancellationToken);

        DbContext.Entry(info).Property(e => e.Gender).CurrentValue = gender;
        
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateLoginAsync(int id, string login, CancellationToken cancellationToken)
    {
        PersonInfo info = await GetPersonAsync(id, cancellationToken);

        DbContext.Entry(info).Property(e => e.Login).CurrentValue = login;
        
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}