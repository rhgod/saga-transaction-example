using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;

namespace Workflow.Api.Handlers;

public class DepositRequestedHandler(IBus bus, ILogger<DepositRequestedHandler> logger)
    : IHandleMessages<DepositRequested>
{
    public async Task Handle(DepositRequested message)
    {
        logger.LogInformation("Fake deposit ok for AccountId {AccountId}, Amount {Amount}", message.AccountId, message.Amount);

        await bus.Publish(new DepositCompleted(message.SagaId, Success: true));
    }
}
