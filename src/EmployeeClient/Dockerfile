FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EmployeeClient.csproj", "./"]
RUN dotnet restore "EmployeeClient.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "EmployeeClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeClient.dll"]
