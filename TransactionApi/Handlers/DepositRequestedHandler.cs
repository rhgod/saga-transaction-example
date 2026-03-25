using Rebus.Bus;
using Rebus.Handlers;
using TransactionApi.Messages;

namespace TransactionApi.Handlers;

public class DepositRequestedHandler(IBus bus, ILogger<DepositRequestedHandler> logger)
    : IHandleMessages<DepositRequested>
{
    public async Task Handle(DepositRequested message)
    {
        logger.LogInformation("Fake deposit ok for AccountId {AccountId}, Amount {Amount}", message.AccountId, message.Amount);

        await bus.Publish(new DepositCompleted(message.SagaId, Success: true));
    }
}
