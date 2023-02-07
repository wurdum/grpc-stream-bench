FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG PROJECT_NAME
WORKDIR /src
COPY ["src/$PROJECT_NAME/$PROJECT_NAME.csproj", "src/$PROJECT_NAME/"]
COPY ["src/GrpcStreamBenchmark.Core/GrpcStreamBenchmark.Core.csproj", "src/GrpcStreamBenchmark.Core/"]
RUN dotnet restore "src/$PROJECT_NAME/$PROJECT_NAME.csproj" -r linux-x64
COPY . .
RUN dotnet build "src/$PROJECT_NAME/$PROJECT_NAME.csproj" -c Release -o /app/build -r linux-x64 --self-contained false --no-restore

FROM build AS publish
ARG PROJECT_NAME
RUN dotnet dev-certs https -ep /app/cert/aspnetapp.pfx -p passw0rd
RUN dotnet publish "src/$PROJECT_NAME/$PROJECT_NAME.csproj" -c Release -o /app/publish -r linux-x64 --self-contained false --no-restore

FROM base AS final
ARG PROJECT_NAME
ENV PROJECT_EXEC="$PROJECT_NAME.dll"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=passw0rd
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/aspnetapp.pfx
ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/cert .
ENTRYPOINT dotnet $PROJECT_EXEC
