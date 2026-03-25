using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;

namespace TransactionApi.Handlers;

public class StartNotificationHandler(IBus bus, ILogger<StartNotificationHandler> logger)
    : IHandleMessages<StartNotification>
{
    public async Task Handle(StartNotification message)
    {
        logger.LogInformation("Fake notification send ok: {Text}", message.Text);

        await bus.Publish(new NotificationCompleted(message.SagaId, Success: true));
    }
}
