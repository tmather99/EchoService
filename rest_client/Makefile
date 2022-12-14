API_PORT = 8080
API_SERVER = 'flash.vmwuem.com'
SEQ_SERVER_URL = 'http://flash.vmwuem.com:5341'

### docker 

build:
	docker build . -t tmather99/rest_client

push:
	docker push tmather99/rest_client

run:
	docker run -e API_SERVER=$(API_SERVER) -e API_PORT=$(API_PORT) -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) --name rest_client --rm tmather99/rest_client

run-path:
	docker run -e API_SERVER=$(API_SERVER) -e API_PORT=$(API_PORT) -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) --name rest_client --rm tmather99/rest_client -f *PathEndpoint* --iterationCount 1

run-query:
	docker run -e API_SERVER=$(API_SERVER) -e API_PORT=$(API_PORT) -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) --name rest_client --rm tmather99/rest_client -f *QueryEndpoint* --iterationCount 1

run-body:
	docker run -e API_SERVER=$(API_SERVER) -e API_PORT=$(API_PORT) -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) --name rest_client --rm tmather99/rest_client -f *BodyContract* --iterationCount 1



### dotnet CLI

dotnet-build:
	dotnet build -c Release


### WCF Rest wsHttpBinding

dotnet-run-path: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=$(API_PORT); dotnet run -c Release -- -f *PathEndpoint* --iterationCount 1

dotnet-run-query: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=$(API_PORT); dotnet run -c Release -- -f *QueryEndpoint* --iterationCount 1

dotnet-run-body: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=$(API_PORT); dotnet run -c Release -- -f *BodyContract* --iterationCount 1





dapr-run-path:
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *RestClientGetEvents* --iterationCount 1



### WebApi Rest controllers


webapi-run-events: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8081; dotnet run -c Release -- -f *WebApiGetEvents* --iterationCount 1

webapi-run-id: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8081; dotnet run -c Release -- -f *WebApiGetEventById* --iterationCount 1


### Dapr RestClient service invoke


dapr-http-events: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *RestClientGetEvents* --iterationCount 1

dapr-http-id: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *RestClientGetEventById* --iterationCount 1


### Dapr SDK service invoke


dapr-sdk-events: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *DaprSDKGetEvents* --iterationCount 1

dapr-sdk-id: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *DaprSDKGetEventById* --iterationCount 1


### Dapr sidecar for unit test debugging

dapr-sidecar: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --dapr-http-port $$env:DAPR_HTTP_PORT 


### Dapr statestore

dapr-statestore: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 --components-path ../dapr/components \
                 -- dotnet run -c Release -- -f *DaprStateStore* --iterationCount 1

### Dapr secrets

dapr-get-secrets: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 --components-path ../dapr/components \
                 -- dotnet run -c Release -- -f *DaprGetSecrets* --iterationCount 1

### Dapr pubsub

dapr-publish: 
	powershell $$env:DAPR_HTTP_PORT=3500; \
		dapr run --app-id rest_client \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *DaprPublishOrder* --iterationCount 1
