FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AuthorizeTransaction.ConsoleApp/AuthorizeTransaction.ConsoleApp.csproj", "AuthorizeTransaction.ConsoleApp/"]
COPY ["AuthorizeTransaction.Domain/AuthorizeTransaction.Domain.csproj", "AuthorizeTransaction.Domain/"]
COPY ["AuthorizeTransaction.Infrastructure/AuthorizeTransaction.Infrastructure.csproj", "AuthorizeTransaction.Infrastructure/"]
COPY ["AuthorizeTransaction.Tests/AuthorizeTransaction.Tests.csproj", "AuthorizeTransaction.Tests/"]
RUN dotnet restore "AuthorizeTransaction.ConsoleApp/AuthorizeTransaction.ConsoleApp.csproj"

COPY . .
WORKDIR "/src/AuthorizeTransaction.ConsoleApp"
RUN dotnet build "AuthorizeTransaction.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorizeTransaction.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizeTransaction.ConsoleApp.dll"]