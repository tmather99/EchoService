FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tcp_server.csproj", "."]
RUN dotnet restore "tcp_server.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "tcp_server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tcp_server.csproj" -c Release -o /app /p:UseAppHost=false

WORKDIR /app
EXPOSE 8089

ENTRYPOINT ["dotnet", "tcp_server.dll"]