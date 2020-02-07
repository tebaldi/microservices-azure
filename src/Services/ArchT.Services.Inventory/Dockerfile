FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY /src/Services/ArchT.Services.Inventory/ Services/ArchT.Services.Inventory/
WORKDIR /src/Services/ArchT.Services.Inventory
RUN dotnet build -c Release -o /app

FROM build AS test
WORKDIR /src
COPY /src/Tests/Services/ArchT.Tests.Services.Inventory/ Tests/Services/ArchT.Tests.Services.Inventory/
WORKDIR /src/Tests/Services/ArchT.Tests.Services.Inventory
RUN dotnet test -c Release --logger "trx;LogFileName=testresults.trx"

FROM build AS publish
WORKDIR /src/Services/ArchT.Services.Inventory
RUN dotnet publish -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ArchT.Services.Inventory.dll"]