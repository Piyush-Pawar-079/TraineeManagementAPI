FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

COPY *.csproj .

RUN dotnet restore 

RUN dotnet publish -c Release -o /app/publish

FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/aspnet:10.0-alpine

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT [ "dotnet", "traineeManagementAPI.dll" ]


