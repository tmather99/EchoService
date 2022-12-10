build:
	docker build . -t tmather99/benchmark

push:
	docker push tmather99/benchmark

seq:
	docker run --name seq.local.com -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
	
run-http:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e HTTP_PORT=8088 --rm -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' tmather99/benchmark -f *CallBasicHttpBinding* --iterationCount 1 -m

run-wshttp:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e HTTP_PORT=8088 --rm -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' tmather99/benchmark -f *CallWsHttpBinding* --iterationCount 1

run-tcp:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e NETTCP_PORT=8090 --rm -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' tmather99/benchmark -f *CallNetTcpBinding* --iterationCount 1

run-cal:
	docker run -e TCP_SERVER='flash.vmwuem.com' -e TCP_PORT=8089 --rm -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' tmather99/benchmark -f *CallCalculator* --iterationCount 1



dotnet-build:
	dotnet build -c Release

dotnet-run-http: 
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; dotnet run -c Release -- -f *CallBasicHttpBinding* --iterationCount 1

dotnet-run-wshttp:
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; dotnet run -c Release -- -f *CallWsHttpBinding* --iterationCount 1

dotnet-run-tcp:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:NETTCP_PORT=8090; dotnet run -c Release -- -f *CallNetTcpBinding* --iterationCount 1

dotnet-run-transport:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:NETTCP_PORT=8090; dotnet run -c Release -- -f *CallNetTcpTransportBinding* --iterationCount 1

dotnet-run-msg:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:NETTCP_PORT=8090; dotnet run -c Release -- -f *CallNetTcpTransportWithMessageCredentialBinding* --iterationCount 1

dotnet-run-cal:
	powershell $$env:TCP_SERVER='127.0.0.1'; $$env:TCP_PORT=8089; dotnet run -c Release -- -f *CallCalculator* --iterationCount 1


### WCF Rest wsHttpBinding

dotnet-run-path: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release -- -f *PathEndpoint* --iterationCount 1

dotnet-run-query: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release -- -f *QueryEndpoint* --iterationCount 1

dotnet-run-body: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release -- -f *BodyContract* --iterationCount 1


### Dapr RestClient service invoke

dapr-run-path:
	powershell $$env:DAPR_HTTP_PORT=3581; \
		dapr run --app-id benchmark \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *DaprPathEndpoint* --iterationCount 1

dapr-run-query:
	powershell $$env:DAPR_HTTP_PORT=3581; \
		dapr run --app-id benchmark \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *DarpQueryEndpoint* --iterationCount 1

dapr-run-body:
	powershell $$env:DAPR_HTTP_PORT=3581; \
		dapr run --app-id benchmark \
                 --dapr-http-port $$env:DAPR_HTTP_PORT \
                 -- dotnet run -c Release -- -f *DaprBodyContract* --iterationCount 1

### K8s ingress endpoint

k8s-run-path: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'rest.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *PathEndpoint* --iterationCount 1

k8s-run-query: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'rest.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *DaprBodyContract* --iterationCount 1

k8s-run-body: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'rest.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *BodyContract* --iterationCount 1


k8s-run-http:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:HTTP_PORT=443; $$env:SEQ_SERVER_URL=\'https://seq.local.com\'; dotnet run -c Release -- -f *CallSecureBasicHttpBinding* --iterationCount 1

k8s-run-wshttp:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:HTTP_PORT=443; $$env:SEQ_SERVER_URL=\'https://seq.local.com\'; dotnet run -c Release -- -f *CallSecureWsHttpBinding* --iterationCount 1

#make scale REPLICAS=2
REPLICAS = 1

scale:
	kubectl scale deployment echo-server-app --replicas=$(REPLICAS)


### Ingres controller


ingress-events: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8083; dotnet run -c Release -- -f *GetEvents* --iterationCount 1

ingress-id: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8083; dotnet run -c Release -- -f *GetEventById* --iterationCount 1

ingress-statestore: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8083; dotnet run -c Release -- -f *StateStore* --iterationCount 1

ingress-secrets: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8083; dotnet run -c Release -- -f *Secrets* --iterationCount 1

ingress-publish: 
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8083; dotnet run -c Release -- -f *Publish* --iterationCount 1



k8s-ingress-events: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'ingress.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *GetEvents* --iterationCount 1

k8s-ingress-id: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'ingress.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *GetEventById* --iterationCount 1

k8s-ingress-statestore: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'ingress.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *StateStore* --iterationCount 1

k8s-ingress-secrets: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'ingress.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *Secrets* --iterationCount 1

k8s-ingress-publish: 
	powershell $$env:API_PROTOCOL=\'https\'; \
               $$env:API_SERVER=\'ingress.local.com\'; \
               $$env:API_PORT=443; \
			   $$env:SEQ_SERVER_URL=\'http://seq.local.com\'; \
               dotnet run -c Release -- -f *Publish* --iterationCount 1
