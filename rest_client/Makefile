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
