FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./Shared/Shared.Application/Shared.Application.csproj             Shared/Shared.Application/
COPY ./Shared/Shared.Domain/Shared.Domain.csproj                       Shared/Shared.Domain/
COPY ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj       Shared/Shared.Infrastructure/

COPY ./NotificationService/NotificationService.Application/NotificationService.Application.csproj 			NotificationService/NotificationService.Application/
COPY ./NotificationService/NotificationService.Domain/NotificationService.Domain.csproj       				NotificationService/NotificationService.Domain/
COPY ./NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj 	NotificationService/NotificationService.Infrastructure/
COPY ./NotificationService/NotificationService.WebUI/NotificationService.WebUI.csproj         				NotificationService/NotificationService.WebUI/

RUN dotnet restore NotificationService/NotificationService.WebUI/NotificationService.WebUI.csproj

WORKDIR /src
COPY ./Shared Shared
COPY ./NotificationService NotificationService

WORKDIR /src/NotificationService/NotificationService.WebUI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CertMailer.NotificationService.WebUI.dll"]
