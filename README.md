# Docker and ASP.NET Core 3.0

A sample code for the presentation "Docker and ASP.NET Core 3.0". There's also an older version of the presentation and different sample, for ASP.NET Core 2.1. You can find it [here](https://github.com/miroslavpopovic/aspnetcore-workshop).

Sample application is taken from my [ASP.NET Core workshop](https://github.com/miroslavpopovic/aspnetcore-workshop).

Some docker and config files taken from (inspired by) [Quick intro to Docker and Docker Compose: Angular, ASP.NET Core and Postgres app](
https://blog.codingmilitia.com/2018/01/31/quick-intro-to-docker-and-docker-compose-angular-aspnetcore-postgres-app) post by [@joaofbantunes](https://github.com/joaofbantunes).

## Prerequisites

Make sure you have the latest Docker for Windows installed.

Since we are using HTTPS inside Docker image, you also need to provide the certificate for Kestrel. To find out the reasoning and read the instructions, check out [this document](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https-development.md).

    docker build -t aspnetdocker2 . -f src/TimeTracker/Dockerfile

    docker run -p 5008:443 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_Kestrel__Certificates__Default__Password="MyDockerAspNetCoreSample#" -e ASPNETCORE_Kestrel__Certificates__Default__Path="./timetracker.pfx" -v "C:\Users\Miroslav\AppData\Roaming\Microsoft\UserSecrets\:/root/.microsoft/usersecrets:ro" -v "C:\Users\Miroslav\AppData\Roaming\ASP.NET\Https:/root/.aspnet/https/" aspnetdocker2


## Running the sample

Run the following console command from the root folder:

    docker-compose build

Following with:

    docker-compose up

## Download

- TODO
