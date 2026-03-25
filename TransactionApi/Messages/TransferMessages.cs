namespace TransactionApi.Messages;

public record StartTransfer(Guid SagaId, int From, int To, decimal Amount);

public record WithdrawalRequested(Guid SagaId, int AccountId, decimal Amount);

public record WithdrawalCompleted(Guid SagaId, bool Success);

public record DepositRequested(Guid SagaId, int AccountId, decimal Amount);

public record DepositCompleted(Guid SagaId, bool Success);
