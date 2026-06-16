FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY cold-track-platform.sln ./
COPY FreshGuard.ColdTrack.Platform/FreshGuard.ColdTrack.Platform.csproj FreshGuard.ColdTrack.Platform/
COPY FreshGuard.ColdTrack.Platform.Tests/FreshGuard.ColdTrack.Platform.Tests.csproj FreshGuard.ColdTrack.Platform.Tests/
RUN dotnet restore cold-track-platform.sln

COPY . .
RUN dotnet publish FreshGuard.ColdTrack.Platform/FreshGuard.ColdTrack.Platform.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 10000

ENTRYPOINT ["sh", "-c", "dotnet FreshGuard.ColdTrack.Platform.dll --urls http://0.0.0.0:${PORT:-10000}"]
