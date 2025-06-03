FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
COPY . /
RUN dotnet publish --configuration Release

FROM mcr.microsoft.com/dotnet/aspnet:7.0  
COPY --from=builder /src/DBMegreat.ConsoleApp/bin/Release/net7.0/publish app/

WORKDIR /app

ENTRYPOINT ["dotnet", "/app/DBMegreat.ConsoleApp.dll"]