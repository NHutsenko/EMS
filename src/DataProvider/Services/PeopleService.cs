using EMS.Protos.DataProvider;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DataProvider.Services;

public sealed class PeopleService : People.PeopleBase
{
    public override Task<StringValue> Add(Person request, ServerCallContext context)
    {
        return Task.FromResult(new StringValue
        {
            Value = Guid.NewGuid().ToString()
        });
    }

    public override Task<Empty> Update(Person request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }

    public override Task<PeopleReply> GetAll(Empty request, ServerCallContext context)
    {
        PeopleReply reply = new()
        {
            Data = { new Person
                {
                    Id = Guid.Empty.ToString(),
                    LastName = "Mock",
                    FirstName = "Mock",
                    SecondName = "Mock",
                    Birthday = new DateTime(1990, 1, 1, 12, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                    Gender = false,
                    Login = "mock_login"
                }
            }
        };
        return Task.FromResult(reply);
    }

    public override Task<Person> GetById(StringValue request, ServerCallContext context)
    {
        Person person = new Person
        {
            Id = Guid.Empty.ToString(),
            LastName = "Mock",
            FirstName = "Mock",
            SecondName = "Mock",
            Birthday = new DateTime(1990, 1, 1, 12, 0, 0, DateTimeKind.Utc).ToTimestamp(),
            Gender = false,
            Login = "mock_login"
        };
        return Task.FromResult(person);
    }
}