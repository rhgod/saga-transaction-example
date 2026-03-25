using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Shared.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost/", "workflow-api"))
    .Sagas(s => s.StoreInMemory())
    .Routing(r => r.TypeBased()
        .Map<WithdrawalRequested>("workflow-api")
        .Map<DepositRequested>("workflow-api")
        .Map<StartNotification>("workflow-api")
        .Map<WithdrawalCompleted>("workflow-api")
        .Map<DepositCompleted>("workflow-api")
        .Map<NotificationCompleted>("workflow-api"))
);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();
