build:
	docker build . -t tmather99/echo_client

push:
	docker push tmather99/echo_client

run:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e HTTP_PORT=8088 -e NETTCP_PORT=8090 --rm tmather99/echo_client

dotnet-build:
	dotnet build -c Release

dotnet-run:
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; $$env:NETTCP_PORT=8090; dotnet run -c Release
