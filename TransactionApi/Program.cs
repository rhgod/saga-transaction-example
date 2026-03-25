using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using TransactionApi.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost/", "transactionApi"))
    .Sagas(s => s.StoreInMemory())
    .Routing(r => r.TypeBased()
        .Map<WithdrawalRequested>("transactionApi")
        .Map<DepositRequested>("transactionApi")
        .Map<StartNotification>("transactionApi")
        .Map<WithdrawalCompleted>("transactionApi")
        .Map<DepositCompleted>("transactionApi")
        .Map<NotificationCompleted>("transactionApi"))
);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();
