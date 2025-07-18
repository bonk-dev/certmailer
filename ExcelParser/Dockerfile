FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

#COPY ExcelParser.sln ./

COPY ./Shared/Shared.Application/Shared.Application.csproj             Shared/Shared.Application/
COPY ./Shared/Shared.Domain/Shared.Domain.csproj                       Shared/Shared.Domain/
COPY ./Shared/Shared.Infrastructure/Shared.Infrastructure.csproj       Shared/Shared.Infrastructure/

COPY ./ExcelParser/ExcelParser.Application/ExcelParser.Application.csproj 		ExcelParser/ExcelParser.Application/
COPY ./ExcelParser/ExcelParser.Domain/ExcelParser.Domain.csproj       			ExcelParser/ExcelParser.Domain/
COPY ./ExcelParser/ExcelParser.Infrastructure/ExcelParser.Infrastructure.csproj 	ExcelParser/ExcelParser.Infrastructure/
COPY ./ExcelParser/ExcelParser.WebUI/ExcelParser.WebUI.csproj         			ExcelParser/ExcelParser.WebUI/

RUN dotnet restore ExcelParser/ExcelParser.WebUI/ExcelParser.WebUI.csproj

WORKDIR /src
COPY ./Shared Shared
COPY ./ExcelParser ExcelParser

WORKDIR /src/ExcelParser/ExcelParser.WebUI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CertMailer.ExcelParser.WebUI.dll"]
