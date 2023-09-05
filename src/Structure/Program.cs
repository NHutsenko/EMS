using EMS.Logging.Extensions;
using EMS.Staff.Extensions;
using EMS.Staff.Models;
using EMS.Staff.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger();

// Add services to the container.
builder.Services.AddGrpc(cfg =>
{
    cfg.EnableDetailedErrors = true;
    cfg.AddServiceLogging();
    cfg.MaxReceiveMessageSize = null;
});

builder.Services.AddDbContext(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<PositionService>();
app.MapGrpcService<TeamService>();

app.Run();