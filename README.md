# ToDoApp

Aplicação de tarefas (ToDo) construída com .NET 9, seguindo uma arquitetura limpa em camadas (API, Application, Domain, Infrastructure) e com cobertura completa de testes.

## 📦 Tecnologias

- ASP.NET Core 9 (Web API)
- xUnit para testes
- EF Core (sem persistência ativa)
- Docker & Docker Compose
- Arquitetura por camadas

## 📚 Documentação da API (Swagger)
A aplicação já vem configurada com o Swagger para facilitar a exploração e o teste dos endpoints da API.

### 🔗 Acesse via navegador:
Após rodar o projeto (local ou via Docker), abra no navegador:

```bash
http://localhost:5000/swagger
```
Isso irá exibir uma interface interativa com todos os endpoints da API, seus parâmetros, tipos de resposta e permite até executar chamadas diretamente da interface.

✅ Exemplo de uso local com Docker:
```bash
docker-compose up --build
```

Depois, acesse:
```bash
http://localhost:5000/swagger
```

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
