using EMS.Extensions;
using EMS.Logging.Extensions;
using EMS.Staff.Extensions;
using EMS.Staff.Interfaces;
using EMS.Staff.Repositories;
using EMS.Staff.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger();

// Add services to the container.
builder.Services.AddGrpcServer();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddScoped<IStaffRepository, StaffRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<StaffService>();

await app.RunAsync();