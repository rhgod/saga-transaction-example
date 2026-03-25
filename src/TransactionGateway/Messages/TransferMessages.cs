namespace TransactionGateway.Messages;

public record StartTransfer(Guid SagaId, int From, int To, decimal Amount);
