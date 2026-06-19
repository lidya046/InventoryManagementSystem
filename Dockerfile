# Tahap build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["InventoryManagementSystem.csproj", "./"]

RUN dotnet restore "InventoryManagementSystem.csproj"

COPY . .

RUN dotnet publish "InventoryManagementSystem.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Tahap runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "InventoryManagementSystem.dll"]