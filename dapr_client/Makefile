API_PORT = 8083
API_SERVER = flash.vmwuem.com
SEQ_SERVER_URL = http://flash.vmwuem.com:5341
VERSION = v1

### docker 

build:
	docker build . -t tmather99/dapr_client:$(VERSION)

push:
	docker push tmather99/dapr_client:$(VERSION)

run:
	docker run -p 8083:8083 \
               -e ASPNETCORE_URLS=http://*:8083 \
               -e ASPNETCORE_ENVIRONMENT="Development" \
               -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) \
			   --name dapr_client \
			   --rm tmather99/dapr_client 

stop:
	docker stop dapr_client

seq:
	docker run --name seq.local.com -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest

dapr-dashboard:
	dapr dashboard -p 8888

zipkin-dashboard:
	powershell start-process http://localhost:9411

dapr-client-swagger:
	powershell start-process http://localhost:8083/swagger

dapr-server-swagger:
	powershell start-process http://localhost:8081/swagger

### metrics

prometheus:
	docker run \
		-p 8090:8090 \
		-v C:/github/EchoService/Dapr/prometheus.yml:/etc/prometheus/prometheus.yml \
		--rm --name prometheus \
		prom/prometheus --config.file=/etc/prometheus/prometheus.yml --web.listen-address=:8090

rm-prometheus:
	docker stop prometheus

### dotnet CLI

dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8083\"; \
               $$env:ASPNETCORE_ENVIRONMENT=\"Development\"; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
	      dotnet run -c Release --urls http://localhost:8083

### Dapr sidecar for unit test debugging

dapr-sidecar: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --dapr-http-port $$env:DAPR_HTTP_PORT 


### Dapr statestore

dapr-run: 
	powershell $$env:ASPNETCORE_URLS=\"http://*:8083\"; \
               $$env:ASPNETCORE_ENVIRONMENT=\"Development\"; \
               $$env:SECRET_STORE_NAME=\"secretstore\"; \
               $$env:DAPR_HTTP_PORT=3500; \
               $$env:SEQ_SERVER_URL=\'$(SEQ_SERVER_URL)\'; \
		dapr run --app-id dapr-client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 --components-path ../dapr/components \
                 --config ../dapr/config.yml \
                 -- dotnet run -c Release --urls http://localhost:8083
