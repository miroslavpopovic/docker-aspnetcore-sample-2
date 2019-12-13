FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
ENV ASPNETCORE_URLS https://+;http://+
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/TimeTracker/TimeTracker.csproj", "TimeTracker/"]
RUN dotnet restore "TimeTracker/TimeTracker.csproj"
COPY ["src/TimeTracker/", "TimeTracker/"]
WORKDIR /src/TimeTracker
RUN dotnet build "TimeTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracker.dll"]
