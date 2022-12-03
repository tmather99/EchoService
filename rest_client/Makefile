build:
	docker build . -t tmather99/rest_client

push:
	docker push tmather99/rest_client

run:
	docker run -e API_SERVER='flash.vmwuem.com' -e API_PORT=8080 -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' --name rest_client --rm tmather99/rest_client

run-path:
	docker run -e API_SERVER='flash.vmwuem.com' -e API_PORT=8080 -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' --name rest_client --rm tmather99/rest_client -f *PathEndpoint* --iterationCount 1

run-query:
	docker run -e API_SERVER='flash.vmwuem.com' -e API_PORT=8080 -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' --name rest_client --rm tmather99/rest_client -f *QueryEndpoint* --iterationCount 1

run-body:
	docker run -e API_SERVER='flash.vmwuem.com' -e API_PORT=8080 -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' --name rest_client --rm tmather99/rest_client -f *BodyContract* --iterationCount 1


dotnet-build:
	dotnet build -c Release

dotnet-run-path: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release -- -f *PathEndpoint* --iterationCount 1

dotnet-run-query: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release -- -f *QueryEndpoint* --iterationCount 1

dotnet-run-body: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release -- -f *BodyContract* --iterationCount 1



webapi-run-events: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8081; dotnet run -c Release -- -f *WebApiGetEvents* --iterationCount 1

webapi-run-id: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8081; dotnet run -c Release -- -f *WebApiGetEventById* --iterationCount 1


dapr-statestore: 
	powershell $$env:DAPR_PORT=3500; \
		dapr run --app-id rest_client \
		         --dapr-http-port 3500 \
				 --components-path ../dapr/components \
				 -- dotnet run -c Release -- -f *DaprStateStoreBodyContract* --iterationCount 1

dapr-get-secrets: 
	powershell $$env:DAPR_PORT=3500; \
		dapr run --app-id rest_client \
		         --dapr-http-port 3500 \
				 --components-path ../dapr/components \
				 -- dotnet run -c Release -- -f *DaprGetSecrets* --iterationCount 1

dapr-publish: 
	powershell $$env:DAPR_PORT=3500; \
		dapr run --app-id rest_client \
		         --dapr-http-port 3500 \
				 -- dotnet run -c Release -- -f *DaprStatePublishOrder* --iterationCount 1

dapr-run-events: 
	powershell $$env:DAPR_PORT=3500; \
		dapr run --app-id rest_client \
		         --dapr-http-port 3500 \
				 -- dotnet run -c Release -- -f *DaprGetEvents* --iterationCount 1

dapr-run-id: 
	powershell $$env:DAPR_PORT=3500; \
		dapr run --app-id rest_client \
		         --dapr-http-port 3500 \
				 -- dotnet run -c Release -- -f *DaprGetEventById* --iterationCount 1
