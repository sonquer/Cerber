FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Accounts/Accounts.Api/Accounts.Api.csproj Services/Accounts/Accounts.Api/
COPY Services/Accounts/Accounts.Domain/Accounts.Domain.csproj Services/Accounts/Accounts.Domain/
COPY Services/Accounts/Accounts.Infrastructure/Accounts.Infrastructure.csproj Services/Accounts/Accounts.Infrastructure/
RUN dotnet restore "Services/Accounts/Accounts.Api/Accounts.Api.csproj"
COPY . .
WORKDIR "/src/Services/Accounts/Accounts.Api"
RUN dotnet build "Accounts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Accounts.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accounts.Api.dll"]
