using Logging.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(cfg =>
{
    cfg.EnableDetailedErrors = true;
    cfg.MaxReceiveMessageSize = null;
    cfg.AddServiceLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();