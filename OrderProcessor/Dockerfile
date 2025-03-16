FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -o build

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /App
COPY --from=build /App/build .
ENTRYPOINT ["dotnet", "OrderProcessor.dll"]