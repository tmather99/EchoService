build:
	docker build . -t tmather99/dapr_server

push:
	docker push tmather99/dapr_server

run:
	docker run  \
	   -p 8081:8081 \
	   -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' \
	   -e ASPNETCORE_ENVIRONMENT='Development' \
	   -e ASPNETCORE_URLS='http://loalhost:8081' \
	   --rm \
	   --name dapr_server tmather99/dapr_server

stop:
	docker stop dapr_server

dotnet-build:
	dotnet build -c Release

dotnet-run:
	dotnet run -c Release --urls http://loalhost:8081

dapr-dashboard:
	dapr dashboard -p 8888

dapr-run:
	powershell $$env:API_PORT=8081; \
		dapr run \
			--app-id catalog \
			--app-port 8081 \
			--dapr-http-port 3501 \
			--components-path ../dapr/components \
			-- dotnet run -c Release --urls http://loalhost:8081

dapr-run-debug:
	powershell $$env:API_PORT=8081; \
		dapr run \
			--app-id catalog \
			--app-port 8081 \
			--dapr-http-port 3501 \
			--components-path ../dapr/components \
			-- dotnet run -c Debug --urls http://loalhost:8081

dapr-run-1:
	powershell $$env:API_PORT=8082; \
		dapr run \
			--app-id catalog \
			--app-port 8082 \
			--dapr-http-port 3502 \
			--components-path ../dapr/components \
			-- dotnet run -c Release --urls http://loalhost:8082
