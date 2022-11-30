FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Benchmark.csproj", "."]
RUN dotnet restore "Benchmark.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Benchmark.csproj" -c Release -o /app

ENTRYPOINT ["dotnet", "bin/Release/net7.0/Benchmark.dll"]