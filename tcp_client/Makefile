run:
	powershell $$env:TCP_SERVER='127.0.0.1'; $$env:TCP_PORT=8089; $$env:SEQ_SERVER_URL=\'http://flash.vmwuem.com:5341\'; .\bin\Release\tcp_client.exe

run-http: 
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; .\bin\Release\tcp_client.exe -f *CallBasicHttpBinding* --iterationCount 1

run-wshttp:
	powershell $$env:ECHO_SERVER='127.0.0.1'; $$env:HTTP_PORT=8088; .\bin\Release\tcp_client.exe -f *CallWsHttpBinding* --iterationCount 1

run-tcp:
	powershell $$env:ECHO_SERVER=\'127.0.0.1\'; $$env:NETTCP_PORT=8090; .\bin\Release\tcp_client.exe -f *CallNetTcpBinding* --iterationCount 1

run-cal:
	powershell $$env:TCP_SERVER='127.0.0.1'; $$env:TCP_PORT=8089; .\bin\Release\tcp_client.exe -f *CallCalculator* --iterationCount 1


k8s-run-http:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:HTTP_PORT=443; $$env:SEQ_SERVER_URL=\'https://seq.local.com\'; .\bin\Release\tcp_client.exe -f *CallSecureBasicHttpBinding* --iterationCount 1

k8s-run-wshttp:
	powershell $$env:ECHO_SERVER=\'echo.local.com\'; $$env:HTTP_PORT=443; $$env:SEQ_SERVER_URL=\'https://seq.local.com\'; .\bin\Release\tcp_client.exe -f *CallSecureWsHttpBinding* --iterationCount 1
