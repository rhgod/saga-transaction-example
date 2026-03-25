using Rebus.Sagas;

namespace TransactionApi.Sagas;

public class NotificationSagaData : SagaData
{
    public string Text { get; set; } = string.Empty;
}
