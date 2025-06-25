FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY TodoApp.API/*.csproj ./TodoApp.API/
COPY TodoApp.Application/*.csproj ./TodoApp.Application/
COPY TodoApp.Domain/*.csproj ./TodoApp.Domain/
COPY TodoApp.Infrastructure/*.csproj ./TodoApp.Infrastructure/
COPY TodoApp.Tests/*.csproj ./TodoApp.Tests/

RUN dotnet restore

COPY . ./
WORKDIR /src/TodoApp.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TodoApp.API.dll"]
