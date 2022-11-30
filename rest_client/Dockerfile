FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["rest_client.csproj", "."]
RUN dotnet restore "rest_client.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "rest_client.csproj" -c Release -o /app /p:UseAppHost=false

EXPOSE 8080

ENTRYPOINT ["dotnet", "bin/Release/net6.0/rest_client.dll"]