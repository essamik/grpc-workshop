FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EmployeeServer.csproj", "./"]
RUN dotnet restore "EmployeeServer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "EmployeeServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
EXPOSE 5000
ENTRYPOINT ["dotnet", "EmployeeServer.dll"]
