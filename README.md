# Core WCF Server and Client

The WCF server uses 

  * BasicHttpBinding
  * WsHttpBinding
  * NetTcpBinding 

and exposes EndPoints for each binding. It uses ASP.NET Core as the host for the services. 
The server also exposes a WSDL endpoint at http://localhost:8088/EchoService. A client can be created using the svcutil tool against the WSDL endpoint. 

```
svcutil.exe http://localhost:8088/EchoService?wsdl
```

The WCF client makes manual calls endpoints with the BasicHttpBinding, WsHttpBinding and NetTcpBindings.
A base address is specified for http which is use as the basis for each of the binding endpoints. 
This base is also used as the URL for WSDL Discovery.  

## echo_server
```
 dotnet run

 docker build . -t tmather99/echo_server
 docker run -p 8088:8088 -p 8443:8443 -p 8090:8090 tmather99/echo_server
```
## echo_client
```
$env:ECHO_SERVER='flash.vmwuem.com'; dotnet run

docker build . -t tmather99/echo_client
docker run -e ECHO_SERVER=flash.vmwuem.com tmather99/echo_client
```

## Load Testing

http://www.wcfstorm.com/wcf/wcfstorm-lite.aspx

![wcfstorm](./wcfstorm.png)


## Benchmark

https://github.com/dotnet/BenchmarkDotNet

```
dotnet  C:\github\EchoService\Bechmark\bin\Release\net6.0\Bechmark.dll -f *CallBasicHttpBinding*
dotnet  C:\github\EchoService\Bechmark\bin\Release\net6.0\Bechmark.dll -f *CallWsHttpBinding*
$env:ECHO_SERVER='flash.vmwuem.com'; dotnet  C:\github\EchoService\Bechmark\bin\Release\net6.0\Bechmark.dll -f *CallNetTcpBinding* --iterationCount 10
```

```
        |               Method |     Mean |     Error |   StdDev |   Median  |    Gen0 | Allocated |
        |--------------------- |---------:|----------:|----------:|---------:|--------:|----------:|
        | CallBasicHttpBinding | 6.163 ms | 0.3810 ms | 1.062 ms  | 5.708 ms | 23.4375 |  93.46 KB |
        | CallWsHttpBinding    | 5.978 ms | 0.2252 ms | 0.6639 ms |          | 23.4375 |  96.61 KB |
        | CallNetTcpBinding    | 7.510 ms | 0.3220 ms | 0.9443 ms | 31.2500  | 15.6250 | 234.18 KB |
```


```
        |            Method.   |     Mean |    Error  |   StdDev  |    Gen0 |    Gen1 | Allocated |
        |--------------------- |---------:|----------:|----------:|--------:|--------:|----------:|
        | CallBasicHttpBinding | 7.647 ms | 0.1367 ms | 0.1212 ms | 15.6250 |         |  93.54 KB |
        | CallWsHttpBinding    | 7.588 ms | 0.1161 ms | 0.1086 ms | 15.6250 |         |  96.67 KB |
        | CallNetTcpBinding    | 12.05 ms | 0.178 ms  | 0.167 ms  | 31.2500 | 15.6250 |  233.9 KB |
```


## tcp_server
```
 dotnet run

 docker build . -t tmather99/tcp_server
 docker run -p 8089:8089 tmather99/tcp_server
```

## tcp_client
```
$env:ECHO_SERVER='flash.vmwuem.com'; dotnet  C:\github\EchoService\Bechmark\bin\Release\net6.0\Bechmark.dll -f *CallNetTcpBinding* --iterationCount 10

        BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
        Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
        .NET SDK=7.0.100
          [Host]     : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2
          Job-DBXIYG : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2

        IterationCount=10

        |            Method |     Mean |    Error |    StdDev |    Gen0 |    Gen1 | Allocated |
        |------------------ |---------:|---------:|----------:|--------:|--------:|----------:|
        | CallNetTcpBinding | 2.707 ms | 1.018 ms | 0.6736 ms | 54.6875 | 11.7188 | 223.64 KB |
```
```
$env:ECHO_SERVER='flash.vmwuem.com'; .\bin\Release\tcp_client.exe -f *CallNetTcpBinding* --iterationCount 10

        BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
        Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
          [Host]     : .NET Framework 4.8.1 (4.8.9105.0), X86 LegacyJIT
          Job-QUHKNC : .NET Framework 4.8.1 (4.8.9105.0), X86 LegacyJIT

        IterationCount=10

        |            Method |     Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
        |------------------ |---------:|----------:|----------:|--------:|-------:|----------:|
        | CallNetTcpBinding | 2.146 ms | 0.4065 ms | 0.2689 ms | 52.7344 | 1.9531 | 223.72 KB |
```
```
// * Legends *
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Gen0      : GC Generation 0 collects per 1000 operations
  Gen1      : GC Generation 1 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ms      : 1 Millisecond (0.001 sec)
```