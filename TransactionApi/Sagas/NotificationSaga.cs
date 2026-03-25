using Rebus.Handlers;
using Rebus.Sagas;
using TransactionApi.Messages;

namespace TransactionApi.Sagas;

public class NotificationSaga(ILogger<NotificationSaga> logger)
    : Saga<NotificationSagaData>,
      IAmInitiatedBy<StartNotification>,
      IHandleMessages<NotificationCompleted>
{
    protected override void CorrelateMessages(ICorrelationConfig<NotificationSagaData> config)
    {
        config.Correlate<StartNotification>(m => m.SagaId, d => d.Id);
        config.Correlate<NotificationCompleted>(m => m.SagaId, d => d.Id);
    }

    public Task Handle(StartNotification message)
    {
        Data.Text = message.Text;

        logger.LogInformation("NotificationSaga started {SagaId}", message.SagaId);

        return Task.CompletedTask;
    }

    public Task Handle(NotificationCompleted message)
    {
        if (!message.Success)
        {
            logger.LogWarning("Notification failed for SagaId {SagaId}", message.SagaId);
            return Task.CompletedTask;
        }

        logger.LogInformation("NotificationSaga completed for SagaId {SagaId}", message.SagaId);

        MarkAsComplete();

        return Task.CompletedTask;
    }
}
