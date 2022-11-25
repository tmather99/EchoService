FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["echo_client.csproj", "."]
RUN dotnet restore "echo_client.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "echo_client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "echo_client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "echo_client.dll"]