API_PORT = 8083
API_SERVER = 'flash.vmwuem.com'
SEQ_SERVER_URL = 'http://flash.vmwuem.com:5341'

### docker 

build:
	docker build . -t tmather99/dapr_client

push:
	docker push tmather99/dapr_client

run:
	docker run -e API_SERVER=$(API_SERVER) -e API_PORT=$(API_PORT) -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) --name dapr_client --rm tmather99/dapr_client




### dotnet CLI

dotnet-build:
	dotnet build -c Release


### Dapr sidecar for unit test debugging

dapr-sidecar: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --dapr-http-port $$env:DAPR_HTTP_PORT 


### Dapr statestore

dapr-run: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 --components-path ../dapr/components \
                 -- dotnet run -c Release
