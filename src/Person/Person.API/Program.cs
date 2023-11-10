using EMS.Extensions;
using EMS.Logging.Extensions;
using EMS.Person.Application.Interfaces;
using EMS.Person.Application.Services;
using EMS.Person.Infrastructure.Extensions;
using EMS.Person.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcServer();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddScoped<IAddPersonDataRepository, AddPersonDataRepository>();
builder.Services.AddScoped<IGetPersonDataRepository, GetPersonDataRepository>();
builder.Services.AddScoped<IUpdatePersonDataRepository, UpdatePersonDataRepository>();

builder.Services.AddLogger();

WebApplication app = builder.Build();

app.MapGet("/", () => "Person API is available. Use proto to communication");

app.MapGrpcService<PersonService>();

await app.RunAsync();