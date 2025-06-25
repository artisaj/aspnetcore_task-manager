# ToDoApp

Aplicação de tarefas (ToDo) construída com .NET 9, seguindo uma arquitetura limpa em camadas (API, Application, Domain, Infrastructure) e com cobertura completa de testes.

## 📦 Tecnologias

- ASP.NET Core 9 (Web API)
- xUnit para testes
- EF Core (sem persistência ativa)
- Docker & Docker Compose
- Arquitetura por camadas

## 🚀 Como rodar

### Usando o .NET SDK

```bash
dotnet build
dotnet test
dotnet run --project TodoApp.API
```

### Usando Docker
```bash
docker-compose up --build
```

A API estará disponível em http://127.0.0.1:5089.

## ✅ Testes
Execute os testes com:

```bash
dotnet test
```
