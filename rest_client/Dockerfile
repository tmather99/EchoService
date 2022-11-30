FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["rest_client.csproj", "."]
RUN dotnet restore "rest_client.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "rest_client.csproj" -c Release -o /app /p:UseAppHost=false

EXPOSE 8080

ENTRYPOINT ["dotnet", "bin/Release/net7.0/rest_client.dll"]