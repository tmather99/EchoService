SEQ_SERVER_URL = http://flash.vmwuem.com:5341
VERSION = v1

build:
	docker build . -t tmather99/dapr_server:$(VERSION)

push:
	docker push tmather99/dapr_server:$(VERSION)

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

zipkin-dashboard:
	powershell start-process http://localhost:9411

dapr-client-swagger:
	powershell start-process http://localhost:8083/swagger

dapr-server-swagger:
	powershell start-process http://localhost:8081/swagger

dapr-run:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8081\"; \
               $$env:ASPNETCORE_ENVIRONMENT=\"Development\"; \
               $$env:SEQ_SERVER_URL=\'$(SEQ_SERVER_URL)\'; \
		dapr run \
            --app-id catalog \
            --app-port 8081 \
            --dapr-http-port 3501 \
            --components-path ../dapr/components \
            --config ../dapr/config.yml \
            -- dotnet run -c Release

dapr-run-debug:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8081\"; \
               $$env:ASPNETCORE_ENVIRONMENT=\"Development\"; \
               $$env:SEQ_SERVER_URL=\'$(SEQ_SERVER_URL)\'; \
		dapr run \
            --app-id catalog \
            --app-port 8081 \
            --dapr-http-port 3501 \
            --components-path ../dapr/components \
            -- dotnet run -c Debug

dapr-run-1:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8082\"; \
               $$env:ASPNETCORE_ENVIRONMENT=\"Development\"; \
               $$env:SEQ_SERVER_URL=\'$(SEQ_SERVER_URL)\'; \
		dapr run \
            --app-id catalog \
            --app-port 8082 \
            --dapr-http-port 3502 \
            --components-path ../dapr/components \
            --config ../dapr/config.yml \
            -- dotnet run -c Release --urls http://*:8082
