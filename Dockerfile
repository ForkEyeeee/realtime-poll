FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY RealTimePolls.App/RealTimePolls.App.csproj RealTimePolls.App/
WORKDIR /App/RealTimePolls.App
RUN dotnet restore

COPY RealTimePolls.App/. ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/RealTimePolls.App/out .

ENTRYPOINT ["dotnet", "RealTimePolls.App.dll"]
