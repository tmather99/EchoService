build:
	docker build . -t tmather99/tcp_server

push:
	docker push tmather99/tcp_server

run:
	docker run -p 8089:8089 -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' tmather99/tcp_server

dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:TCP_SERVER='127.0.0.1'; $$env:TCP_PORT=8089; dotnet run -c Release
