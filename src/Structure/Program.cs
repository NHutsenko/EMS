using EMS.Extensions;
using EMS.Logging.Extensions;
using EMS.Structure.Extensions;
using EMS.Structure.Interfaces;
using EMS.Structure.Repositories;
using EMS.Structure.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger();

// Add services to the container.
builder.Services.AddGrpcServer();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<PositionService>();
app.MapGrpcService<TeamService>();

await app.RunAsync();