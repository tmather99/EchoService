API_PORT = 8083
API_SERVER = flash.vmwuem.com
SEQ_SERVER_URL = http://flash.vmwuem.com:5341

### docker 

build:
	docker build . -t tmather99/dapr_client

push:
	docker push tmather99/dapr_client

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
		dapr run --app-id dapr_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 --components-path ../dapr/components \
                 -- dotnet run -c Release --urls http://localhost:8083
