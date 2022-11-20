FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 8088
EXPOSE 8443
EXPOSE 8090

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["echo_server.csproj", "."]
RUN dotnet restore "echo_server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "echo_server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "echo_server.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "echo_server.dll"]