FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY NBCZ.Api/NBCZ.Api.csproj NBCZ.Api/
RUN dotnet restore NBCZ.Api/NBCZ.Api.csproj
COPY . .
WORKDIR /src/NBCZ.Api
RUN dotnet build NBCZ.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NBCZ.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NBCZ.Api.dll"]
