build:
	docker build . -t tmather99/rest_server

push:
	docker push tmather99/rest_server

run:
	docker run -p 8080:8080 -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' --rm --name rest_server tmather99/rest_server

stop:
	docker stop rest_server

dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:API_SERVER='127.0.0.1'; $$env:API_PORT=8080; dotnet run -c Release
