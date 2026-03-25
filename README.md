# saga-example-project

SAGA pattern demo using .NET 8 microservices and RabbitMQ messaging.

## Amaç

- SAGA pattern'ını microservice messaging ile göstermek
- İki ayrı ASP.NET Core Web API süreci (TransactionGateway + TransactionApi)
- Rebus mesaj kuyruğu ve RabbitMQ transport
- In-memory saga state persistence (DB yok)

## Proje Yapısı

Her servis tamamen bağımsızdır; ortak kütüphane yoktur. Her servis kendi mesaj kontratlarını kendi içinde tanımlar. Her projenin kendi `.gitignore` dosyası vardır.

```
TransactionGateway/              – Bağımsız .NET Web API
  .gitignore
  TransactionGateway.slnx
  TransactionGateway.csproj      – HTTP alır, mesaj publish eder (queue: gateway-api)
  Messages/                      – Sadece bu servisin publish ettiği kontratlar
  Controllers/

TransactionApi/                  – Bağımsız .NET Web API
  .gitignore
  TransactionApi.slnx
  TransactionApi.csproj          – Mesajları consume eder, SAGA'ları yönetir (queue: workflow-api)
  Messages/                      – Bu servisin işlediği tüm kontratlar
  Sagas/
  Handlers/
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
# 1. TransactionApi'yi başlat (önce)
dotnet run --project TransactionApi/TransactionApi.csproj

# 2. TransactionGateway'i başlat (ayrı terminalde)
dotnet run --project TransactionGateway/TransactionGateway.csproj

# 3. Transfer isteği gönder
curl -X POST http://localhost:5236/transfer \
  -H "Content-Type: application/json" \
  -d '{"from": 1, "to": 2, "amount": 100}'
```

## Beklenen Log Akışı (TransactionApi)

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
