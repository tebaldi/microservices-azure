FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY /src/Services/ArchT.Services.OrderProcessor/ Services/ArchT.Services.OrderProcessor/
WORKDIR /src/Services/ArchT.Services.OrderProcessor
RUN dotnet build -c Release -o /app

FROM build AS publish
WORKDIR /src/Services/ArchT.Services.OrderProcessor
RUN dotnet publish -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ArchT.Services.OrderProcessor.dll"]