FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Availability/Availability.Worker/Availability.Worker.csproj Services/Availability/Availability.Worker/
COPY Services/Availability/Availability.Domain/Availability.Domain.csproj Services/Availability/Availability.Domain/
COPY Services/Availability/Availability.Infrastructure/Availability.Infrastructure.csproj Services/Availability/Availability.Infrastructure/
RUN dotnet restore "Services/Availability/Availability.Worker/Availability.Worker.csproj"
COPY . .
WORKDIR "/src/Services/Availability/Availability.Worker"
RUN dotnet build "Availability.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Availability.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Availability.Worker.dll"]
