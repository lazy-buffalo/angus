FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 55379
EXPOSE 44384

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY LazyBuffalo.Angus/LazyBuffalo.Angus.csproj LazyBuffalo.Angus/
RUN dotnet restore LazyBuffalo.Angus/LazyBuffalo.Angus.csproj
COPY . .
WORKDIR /src/LazyBuffalo.Angus
RUN dotnet build LazyBuffalo.Angus.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish LazyBuffalo.Angus.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LazyBuffalo.Angus.dll"]
