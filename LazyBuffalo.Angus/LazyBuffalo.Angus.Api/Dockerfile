FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55379
EXPOSE 44384

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY LazyBuffalo.Angus.Api/LazyBuffalo.Angus.Api.csproj LazyBuffalo.Angus.Api/
RUN dotnet restore LazyBuffalo.Angus.Api/LazyBuffalo.Angus.Api.csproj
COPY . .
WORKDIR /src/LazyBuffalo.Angus.Api
RUN dotnet build LazyBuffalo.Angus.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish LazyBuffalo.Angus.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LazyBuffalo.Angus.Api.dll"]
