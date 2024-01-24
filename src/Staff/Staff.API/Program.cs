using EMS.Extensions;
using EMS.Logging.Extensions;
using EMS.Staff.Infrastructure;
using EMS.Staff.Infrastructure.Extensions;
using EMS.Staff.Application.Interfaces;
using EMS.Staff.Application.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger();

// Add services to the container.
builder.Services.AddGrpcServer();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

builder.Services.AddGrpcClients(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Staff API is available. Use proto to communication");

app.MapGrpcService<StaffService>();

await app.RunAsync();