# saga-example-project

SAGA pattern demo using .NET 8 microservices and RabbitMQ messaging.

## Amaç

- SAGA pattern'ını microservice messaging ile göstermek
- İki ayrı ASP.NET Core Web API süreci (Gateway.Api + Workflow.Api)
- Rebus mesaj kuyruğu ve RabbitMQ transport
- In-memory saga state persistence (DB yok)

## Proje Yapısı

```
saga-example-project.slnx
src/
  Shared/          – Paylaşılan mesaj tipleri (StartTransfer, WithdrawalRequested, vb.)
  Gateway.Api/     – HTTP alır, mesaj publish eder (queue: gateway-api)
  Workflow.Api/    – Mesajları consume eder, SAGA'ları yönetir (queue: workflow-api)
```

## Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Docker (RabbitMQ için)

## RabbitMQ'yu Başlatma

```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

## Çalıştırma

```bash
# 1. Workflow.Api'yi başlat (önce)
dotnet run --project src/Workflow.Api

# 2. Gateway.Api'yi başlat (ayrı terminalde)
dotnet run --project src/Gateway.Api

# 3. Transfer isteği gönder
curl -X POST http://localhost:5236/transfer \
  -H "Content-Type: application/json" \
  -d '{"from": 1, "to": 2, "amount": 100}'
```

## Beklenen Log Akışı (Workflow.Api)

```
TransferSaga started <sagaId>
Fake withdraw ok for AccountId 1, Amount 100
Withdrawal completed for SagaId <sagaId>
Fake deposit ok for AccountId 2, Amount 100
Deposit completed, TransferSaga completed for SagaId <sagaId>
NotificationSaga started <sagaId>
Fake notification send ok: Transfer 100 from 1 to 2 completed
NotificationSaga completed for SagaId <sagaId>
```

