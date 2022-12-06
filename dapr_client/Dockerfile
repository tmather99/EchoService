FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["dapr_client.csproj", "."]
RUN dotnet restore "dapr_client.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "dapr_client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dapr_client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dapr_client.dll"]