using EMS.Extensions;
using EMS.Logging.Extensions;
using EMS.Structure.Position.Application.Interfaces;
using EMS.Structure.Position.Application.Services;
using EMS.Structure.Position.Infrastructure;
using EMS.Structure.Position.Infrastructure.Extensions;
using EMS.Structure.Team.Application.Interfaces;
using EMS.Structure.Team.Application.Services;
using EMS.Structure.Team.Infrastructure;
using EMS.Structure.Team.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger();

// Add services to the container.
builder.Services.AddGrpcServer();

builder.Services.AddPositionDbContext(builder.Configuration);
builder.Services.AddTeamDbContext(builder.Configuration);

builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<PositionService>();
app.MapGrpcService<TeamService>();

await app.RunAsync();