using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Shared.Messages;

namespace Workflow.Api.Sagas;

public class TransferSaga(IBus bus, ILogger<TransferSaga> logger)
    : Saga<TransferSagaData>,
      IAmInitiatedBy<StartTransfer>,
      IHandleMessages<WithdrawalCompleted>,
      IHandleMessages<DepositCompleted>
{
    protected override void CorrelateMessages(ICorrelationConfig<TransferSagaData> config)
    {
        config.Correlate<StartTransfer>(m => m.SagaId, d => d.Id);
        config.Correlate<WithdrawalCompleted>(m => m.SagaId, d => d.Id);
        config.Correlate<DepositCompleted>(m => m.SagaId, d => d.Id);
    }

    public async Task Handle(StartTransfer message)
    {
        Data.From = message.From;
        Data.To = message.To;
        Data.Amount = message.Amount;

        logger.LogInformation("TransferSaga started {SagaId}", message.SagaId);

        await bus.Publish(new WithdrawalRequested(message.SagaId, message.From, message.Amount));
    }

    public async Task Handle(WithdrawalCompleted message)
    {
        if (!message.Success)
        {
            logger.LogWarning("Withdrawal failed for SagaId {SagaId}", message.SagaId);
            return;
        }

        logger.LogInformation("Withdrawal completed for SagaId {SagaId}", message.SagaId);

        await bus.Publish(new DepositRequested(message.SagaId, Data.To, Data.Amount));
    }

    public async Task Handle(DepositCompleted message)
    {
        if (!message.Success)
        {
            logger.LogWarning("Deposit failed for SagaId {SagaId}", message.SagaId);
            return;
        }

        logger.LogInformation("Deposit completed, TransferSaga completed for SagaId {SagaId}", message.SagaId);

        MarkAsComplete();

        await bus.Publish(new StartNotification(
            message.SagaId,
            $"Transfer {Data.Amount} from {Data.From} to {Data.To} completed"));
    }
}
