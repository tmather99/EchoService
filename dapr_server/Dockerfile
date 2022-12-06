FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["dapr_server.csproj", "."]
RUN dotnet restore "dapr_server.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "dapr_server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dapr_server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dapr_server.dll"]