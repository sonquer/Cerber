FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Gateway/Gateway.Api/Gateway.Api.csproj Services/Gateway/Gateway.Api/
RUN dotnet restore "Services/Gateway/Gateway.Api/Gateway.Api.csproj"
COPY . .
WORKDIR "/src/Services/Gateway/Gateway.Api"
RUN dotnet build "Gateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Api.dll"]
