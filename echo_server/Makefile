build:
	docker build . -t tmather99/echo_server

push:
	docker push tmather99/echo_server

run:
	docker run \
	   -p 8088:8088 \
	   -p 8443:8443 \
	   -p 8090:8090 \
	   -e SEQ_SERVER_URL='http://flash.vmwuem.com:5341' \
	   --rm \
	   --name echo_server tmather99/echo_server

stop:
	docker stop echo_server

dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:HTTP_PORT=8088; $$env:NETTCP_PORT=8090; dotnet run -c Release

seq:
	docker run --name seq.local.com -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest