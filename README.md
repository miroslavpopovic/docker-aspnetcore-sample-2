# Docker and ASP.NET Core 3.0

A sample code for the presentation "Docker and ASP.NET Core 3.0". There's also an older version of the presentation and different sample, for ASP.NET Core 2.1. You can find it [here](https://github.com/miroslavpopovic/aspnetcore-workshop).

Sample application is taken from my [ASP.NET Core workshop](https://github.com/miroslavpopovic/aspnetcore-workshop). Some docker and config files taken from (inspired by) [Quick intro to Docker and Docker Compose: Angular, ASP.NET Core and Postgres app](
https://blog.codingmilitia.com/2018/01/31/quick-intro-to-docker-and-docker-compose-angular-aspnetcore-postgres-app) post by [@joaofbantunes](https://github.com/joaofbantunes).

The instructions below are assuming you are using Windows operating system as a host.

## Prerequisites

1. [Docker Desktop](https://docs.docker.com/docker-for-windows/install/)
2. [Visual Studio 2019+](https://visualstudio.microsoft.com/vs/)
3. Alternatively, [Visual Studio Code](https://code.visualstudio.com/) with
   1. [C# for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
   2. [Docker for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
4. HTTPS certificate - for serving HTTPS from inside Linux Docker image

### Creating and adding HTTPS Certificate

*NOTE: Please note that Visual Studio will initialize everything that's needed for you. If you are not using Visual Studio or have issues with running HTTPS from the containers, read on.*

To find out the reasoning and read the instructions, check out [this document](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https-development.md).

The following assumes you are using PowerShell for running commands. If you are using `cmd`, provide environment variables in this format `%USERPROFILE%` instead of `$env:USERPROFILE`.

To create certificate, run the following:

    dotnet dev-certs https -ep $env:APPDATA\ASP.NET\Https\TimeTracker.pfx -p TimeTracker#

The value `TimeTracker#` is a password for certificate. You can use a different value, but remember to use it in the following calls too. Now you need to add that password to user secrets, so Kestrel server can use the certificate. Run the following from the root repository folder:

    dotnet user-secrets -p src\TimeTracker\TimeTracker.csproj set "Kestrel:Certificates:Development:Password" "TimeTracker#"

## Running the sample

Run this command from the root folder:

    docker-compose build

And then:

    docker-compose up

The application is now accessible from the following URLs:
 - https://localhost:44397/swagger/

## Running individual services

In most cases, you should run using docker compose, but if you want to run individual services, here's how you can do it.

### TimeTracker API

To run the TimeTracker API project with Docker, first create an image from the root folder:

    docker build -t timetracker-api-1 . -f src/TimeTracker/Dockerfile

Run with PowerShell:

    docker run -p 5008:443 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS="https://+" -v $env:APPDATA/Microsoft/UserSecrets/:/root/.microsoft/usersecrets:ro -v $env:APPDATA/ASP.NET/Https:/root/.aspnet/https/ timetracker-api-1

Or run with `cmd`

    docker run -p 5008:443 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS="https://+" -v %APPDATA%/Microsoft/UserSecrets/:/root/.microsoft/usersecrets:ro -v %APPDATA%/ASP.NET/Https:/root/.aspnet/https/ timetracker-api-1

The command above will correctly map the host machine certificate folder and user secrets folder to locations in Linux based container that Kestrel expects. You should now be able to browse the API (Swagger docs) on this URL: https://localhost:5008/swagger/

### TimeTracker Client

Build the image:

    docker build -t timetracker-client-1 . -f src/TimeTracker.Client/build/Dockerfile

Run the image:

    docker run -p 5009:80 timetracker-client-1

## Download

- TODO
