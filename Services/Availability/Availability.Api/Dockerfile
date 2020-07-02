#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Availability/Availability.Api/Availability.Api.csproj Services/Availability/Availability.Api/
COPY Services/Availability/Availability.Domain/Availability.Domain.csproj Services/Availability/Availability.Domain/
COPY Services/Availability/Availability.Infrastructure/Availability.Infrastructure.csproj Services/Availability/Availability.Infrastructure/
RUN dotnet restore "Services/Availability/Availability.Api/Availability.Api.csproj"
COPY . .
WORKDIR "/src/Services/Availability/Availability.Api"
RUN dotnet build "Availability.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Availability.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Availability.Api.dll"]
