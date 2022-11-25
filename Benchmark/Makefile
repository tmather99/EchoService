build:
	docker build . -t tmather99/benchmark

push:
	docker push tmather99/benchmark

run-http:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e HTTP_PORT=8088 tmather99/benchmark -f *CallBasicHttpBinding* --iterationCount 1 -m

run-wshttp:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e HTTP_PORT=8088 tmather99/benchmark -f *CallWsHttpBinding* --iterationCount 1

run-tcp:
	docker run -e ECHO_SERVER='flash.vmwuem.com' -e NETTCP_PORT=8090 tmather99/benchmark -f *CallNetTcpBinding* --iterationCount 1

run-cal:
	docker run -e TCP_SERVER='flash.vmwuem.com' -e TCP_PORT=8089 tmather99/benchmark -f *CallCalculator* --iterationCount 1

dotnet-build:
	dotnet build -c Release

dotnet-run-http: 
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; dotnet .\bin\Release\net6.0\Benchmark.dll -f *CallBasicHttpBinding* --iterationCount 1

dotnet-run-wshttp:
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; dotnet .\bin\Release\net6.0\Benchmark.dll -f *CallWsHttpBinding* --iterationCount 1

dotnet-run-tcp:
	powershell $$env:ECHO_SERVER=\'127.0.0.1\'; $$env:NETTCP_PORT=8090; dotnet .\bin\Release\net6.0\Benchmark.dll -f *CallNetTcpBinding* --iterationCount 1

dotnet-run-cal:
	powershell $$env:TCP_SERVER='127.0.0.1'; $$env:TCP_PORT=8089; dotnet .\bin\Release\net6.0\Benchmark.dll -f *CallCalculator* --iterationCount 1