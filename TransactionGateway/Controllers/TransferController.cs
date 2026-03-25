using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using TransactionGateway.Messages;

namespace TransactionGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferController(IBus bus) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TransferRequest request)
    {
        var sagaId = Guid.NewGuid();
        await bus.Publish(new StartTransfer(sagaId, request.From, request.To, request.Amount));
        return Ok(new { sagaId, status = "Published" });
    }
}

public record TransferRequest(int From, int To, decimal Amount);
