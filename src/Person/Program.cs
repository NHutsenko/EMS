using EMS.Extensions;
using EMS.Logging.Extensions;
using EMS.Person.Extensions;
using EMS.Person.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcServer();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddLogger();

WebApplication app = builder.Build();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<PersonService>();

await app.RunAsync();