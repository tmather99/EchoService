FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["rest_server.csproj", "."]
RUN dotnet restore "rest_server.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "rest_server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rest_server.csproj" -c Release -o /app /p:UseAppHost=false

WORKDIR /app
EXPOSE 8080

ENTRYPOINT ["dotnet", "rest_server.dll"]
