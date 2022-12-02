dotnet-build:
	dotnet build -c Release

dotnet-run:
	dotnet run -c Release

dapr-run:
	powershell $$env:API_PORT=8081; \
		dapr run \
			--app-id catalog \
			--app-port 8081 \
			--dapr-http-port 3501 \
			--components-path ../dapr/components \
			-- dotnet run -c Release

dapr-run-1:
	powershell $$env:API_PORT=8082; \
		dapr run \
			--app-id catalog \
			--app-port 8082 \
			--dapr-http-port 3502 \
			--components-path ../dapr/components \
			-- dotnet run -c Release
