# CertMailer

A microservice app designed to generate course completion certificates and email them to the participants.

## Projects

The app consists of four .NET microservices (one of which is an Ocelot-based API Gateway):

- **ExcelParser**: entry point for the Excel spreadsheets. Responsible for parsing the spreadsheets 
  and reporting the job status. 
- **CertificateGen**: listens for the `ExcelParsed` event on the RabbitMQ bus, generates PDF certificates
  and sends events for each certificate generated.
- **NotificationService**: listens for the `CertificateGenerated` event on the bus and queues emails
  using Hangfire.
- **ApiGateway**: single project app acting as a bridge between the consumers and the three microservices.
  - the ApiGateway also serves a basic Bootstrap [frontend app](ApiGateway/wwwroot) (vanilla JS)

_All services follow the Clean Architecture pattern, except the ApiGateway._

## Limitations

At the time of writing this README, the app has some limitations:
- It's not possible to add new templates — only modify the existing defaults
- The ExcelParser service doesn't check if the mail and cert templates actually exist
- The ExcelParser service probably shouldn't be responsible for manging the job status
- It's not possible to clear or cancel jobs
- The API gateway also serves the frontend app
- The CertificateGen service doesn't cache the background image

## Run

You can run the app using docker-compose (see [docker-compose.yml](docker-compose.yml) 
and [configs](docker-config)):
```shell
docker-compose up -d
```

The compose project includes a dev SMTP server ([smtp4dev](https://github.com/rnwood/smtp4dev/))
which is exposed at http://0.0.0.0:5005. 
The app itself will be available at http://0.0.0.0:5004. 

### Sample files

Sample .XLSX files are in [samples](samples).
