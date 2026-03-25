using Rebus.Sagas;

namespace TransactionApi.Sagas;

public class TransferSagaData : SagaData
{
    public int From { get; set; }
    public int To { get; set; }
    public decimal Amount { get; set; }
}
