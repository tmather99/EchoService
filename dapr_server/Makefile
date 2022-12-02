dotnet-build:
	dotnet build -c Release

dotnet-run:
	dotnet run -c Release

dapr-run:
	dapr run --app-id catalog --app-port 8081 --dapr-http-port 3501 --components-path ../dapr/components -- dotnet run --configuration Release
