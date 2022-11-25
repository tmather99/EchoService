FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Benchmark.csproj", "."]
RUN dotnet restore "Benchmark.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Benchmark.csproj" -c Release -o /app

ENTRYPOINT ["dotnet", "bin/Release/net6.0/Benchmark.dll"]