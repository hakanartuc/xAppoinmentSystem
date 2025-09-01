# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/xAppoinmentSystem.API/xAppoinmentSystem.API.csproj", "src/xAppoinmentSystem.API/"]
COPY ["src/xAppoinmentSystem.Application/xAppoinmentSystem.Application.csproj", "src/xAppoinmentSystem.Application/"]
COPY ["src/xAppoinmentSystem.Domain/xAppoinmentSystem.Domain.csproj", "src/xAppoinmentSystem.Domain/"]
COPY ["src/xAppoinmentSystem.Infrastructure/xAppoinmentSystem.Infrastructure.csproj", "src/xAppoinmentSystem.Infrastructure/"]

RUN dotnet restore "src/xAppoinmentSystem.API/xAppoinmentSystem.API.csproj"

# Copy all files and build
COPY . .
WORKDIR "/src/src/xAppoinmentSystem.API"
RUN dotnet build "xAppoinmentSystem.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "xAppoinmentSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage with SDK for EF migrations
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Install postgresql-client for pg_isready
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "xAppoinmentSystem.API.dll"]