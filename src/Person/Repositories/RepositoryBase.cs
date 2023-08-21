using EMS.Person.Context;
using EMS.Person.Models;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Repositories;

public abstract class RepositoryBase<T>
{
    protected readonly PersonContext DbContext;

    protected RepositoryBase(PersonContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<PersonInfo> GetPersonAsync(int id, CancellationToken cancellationToken)
    {
        PersonInfo? info = await DbContext.People.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (info is null)
        {
            throw new NotFoundException($"Person with id: {id} not found");
        }

        return info;
    }
}