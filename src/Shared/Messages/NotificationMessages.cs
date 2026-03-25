namespace Shared.Messages;

public record StartNotification(Guid SagaId, string Text);

public record NotificationCompleted(Guid SagaId, bool Success);
