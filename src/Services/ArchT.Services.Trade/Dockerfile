FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY /src/Services/ArchT.Services.Trade/ Services/ArchT.Services.Trade/
WORKDIR /src/Services/ArchT.Services.Trade
RUN dotnet build -c Release -o /app

FROM build AS test
WORKDIR /src
COPY /src/Tests/Services/ArchT.Tests.Services.Trade/ Tests/Services/ArchT.Tests.Services.Trade/
WORKDIR /src/Tests/Services/ArchT.Tests.Services.Trade
RUN dotnet test -c Release --logger "trx;LogFileName=testresults.trx"

FROM build AS publish
WORKDIR /src/Services/ArchT.Services.Trade
RUN dotnet publish -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ArchT.Services.Trade.dll"]