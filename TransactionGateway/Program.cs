using Rebus.Config;
using Rebus.Routing.TypeBased;
using TransactionGateway.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost/", "transactionGateway"))
    .Routing(r => r.TypeBased().Map<StartTransfer>("transactionApi"))
);

var app = builder.Build();

app.MapControllers();
app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();
