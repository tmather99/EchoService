API_SERVER = 'flash.vmwuem.com'
SEQ_SERVER_URL = 'http://flash.vmwuem.com:5341'
VERSION = v1

build:
	docker build . -t tmather99/rest_server:$(VERSION)

push:
	docker push tmather99/rest_server:$(VERSION)

run:
	docker run -p 8080:8080 \
               -e ASPNETCORE_URLS=http://*:8080 \
               -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) \
			   --name rest_server \
			   --rm tmather99/rest_server 
stop:
	docker stop rest_server


dapr-dashboard:
	dapr dashboard -p 8888

zipkin-dashboard:
	powershell start-process http://localhost:9411

dapr-rest-swagger:
	powershell start-process http://localhost:8080/swagger


dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8080\"; \
	      dotnet run -c Release --urls http://loalhost:8080


dapr-run:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8080\"; \
               $$env:ASPNETCORE_ENVIRONMENT=\"Development\"; \
               $$env:SEQ_SERVER_URL=\'$(SEQ_SERVER_URL)\'; \
		dapr run \
            --app-id rest-server \
            --app-port 8080 \
            --dapr-http-port 3580 \
            --components-path ../dapr/components \
            --config ../dapr/config.yml \
            -- dotnet run -c Release
