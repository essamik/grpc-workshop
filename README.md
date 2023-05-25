**Containerization**

Running the server in the container requires some extra steps involving security certificate because of how Docker handles TLS.
```
cd .\src\EmployeeServer\
docker build --tag employee-server .
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p xxx
dotnet dev-certs https --trust
docker run --rm -it -p 5000:80 -p 5001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="xxx" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v $env:USERPROFILE\.aspnet\https:/https/ employee-server
```
And then switch the host:port in the client to https://localhost:5001