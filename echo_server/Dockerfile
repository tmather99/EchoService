FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["echo_server.csproj", "."]
RUN dotnet restore "echo_server.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "echo_server.csproj" -c Release -o /app/build /p:UseAppHost=false

FROM build AS publish
RUN dotnet publish "echo_server.csproj" -c Release -o /app

WORKDIR /app
EXPOSE 8088
EXPOSE 8443
EXPOSE 8090

ENTRYPOINT ["dotnet", "echo_server.dll"]