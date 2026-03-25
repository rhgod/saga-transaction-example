using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;

namespace TransactionApi.Handlers;

public class WithdrawalRequestedHandler(IBus bus, ILogger<WithdrawalRequestedHandler> logger)
    : IHandleMessages<WithdrawalRequested>
{
    public async Task Handle(WithdrawalRequested message)
    {
        logger.LogInformation("Fake withdraw ok for AccountId {AccountId}, Amount {Amount}", message.AccountId, message.Amount);

        await bus.Publish(new WithdrawalCompleted(message.SagaId, Success: true));
    }
}
