# Docker configuration inspired by https://chrissainty.com/containerising-blazor-applications-with-docker-containerising-a-blazor-webassembly-app/

# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/TimeTracker.Client/TimeTracker.Client.csproj", "TimeTracker.Client/"]
RUN dotnet restore "TimeTracker.Client/TimeTracker.Client.csproj"
COPY ["src/TimeTracker.Client/", "TimeTracker.Client/"]
WORKDIR /src/TimeTracker.Client
RUN dotnet build "TimeTracker.Client.csproj" -c Release -o /app/build

# STAGE 2: Publish
FROM build AS publish
RUN dotnet publish "TimeTracker.Client.csproj" -c Release -o /app/publish

# STAGE 3: Deploy
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/TimeTracker.Client/dist .
COPY ["src/TimeTracker.Client/build/nginx.conf", "/etc/nginx/nginx.conf"]
