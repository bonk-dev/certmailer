FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ./Directory.Packages.props ./
COPY ./Directory.Build.props ./
COPY ./global.json ./

COPY ./ApiGateway/ApiGateway.csproj ApiGateway/

RUN dotnet restore ApiGateway/ApiGateway.csproj

WORKDIR /src
COPY ./ApiGateway ApiGateway

WORKDIR /src/ApiGateway
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CertMailer.ApiGateway.dll"]
