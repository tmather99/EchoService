FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["echo_client.csproj", "."]
RUN dotnet restore "echo_client.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "echo_client.csproj" -c Release -o /app/build /p:UseAppHost=false

FROM build AS publish
RUN dotnet publish "echo_client.csproj" -c Release -o /app

WORKDIR /app

ENTRYPOINT ["dotnet", "echo_client.dll"]