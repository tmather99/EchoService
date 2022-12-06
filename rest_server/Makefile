API_SERVER = 'flash.vmwuem.com'
SEQ_SERVER_URL = 'http://flash.vmwuem.com:5341'

build:
	docker build . -t tmather99/rest_server

push:
	docker push tmather99/rest_server

run:
	docker run -p 8080:8080 \
               -e ASPNETCORE_URLS=http://*:8080 \
               -e SEQ_SERVER_URL=$(SEQ_SERVER_URL) \
			   --name rest_server \
			   --rm tmather99/rest_server 
stop:
	docker stop rest_server

dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:ASPNETCORE_URLS=\"http://*:8080\"; \
	      dotnet run -c Release --urls http://loalhost:8080
