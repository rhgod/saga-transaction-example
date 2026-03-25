using Rebus.Sagas;

namespace Workflow.Api.Sagas;

public class NotificationSagaData : SagaData
{
    public string Text { get; set; } = string.Empty;
}
