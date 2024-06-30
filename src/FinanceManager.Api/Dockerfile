FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["FinanceManager.sln", "."]
COPY ["./FinanceManager.Api/*.csproj", "./FinanceManager.Api/"]
COPY ["./FinanceManager.Business/*.csproj", "./FinanceManager.Business/"]
COPY ["./FinanceManager.Data/*.csproj", "./FinanceManager.Data/"]

RUN dotnet restore

# copy everything else and build app
COPY ["./FinanceManager.Api/.", "./FinanceManager.Api/"]
COPY ["./FinanceManager.Business/.", "./FinanceManager.Business/"]
COPY ["./FinanceManager.Data/.", "./FinanceManager.Data/"]

WORKDIR /app/FinanceManager.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "FinanceManager.Api.dll"]