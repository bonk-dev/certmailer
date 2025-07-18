FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./Shared/Shared.Application/Shared.Application.csproj             Shared/Shared.Application/
COPY ./Shared/Shared.Domain/Shared.Domain.csproj                       Shared/Shared.Domain/
COPY ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj       Shared/Shared.Infrastructure/

COPY ./CertificateGen/CertificateGen.Application/CertificateGen.Application.csproj 		    CertificateGen/CertificateGen.Application/
COPY ./CertificateGen/CertificateGen.Domain/CertificateGen.Domain.csproj       			    CertificateGen/CertificateGen.Domain/
COPY ./CertificateGen/CertificateGen.Infrastructure/CertificateGen.Infrastructure.csproj 	CertificateGen/CertificateGen.Infrastructure/
COPY ./CertificateGen/CertificateGen.WebUI/CertificateGen.WebUI.csproj         			    CertificateGen/CertificateGen.WebUI/

RUN dotnet restore CertificateGen/CertificateGen.WebUI/CertificateGen.WebUI.csproj

WORKDIR /src
COPY ./Shared Shared
COPY ./CertificateGen CertificateGen

WORKDIR /src/CertificateGen/CertificateGen.WebUI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CertMailer.CertificateGen.WebUI.dll"]
