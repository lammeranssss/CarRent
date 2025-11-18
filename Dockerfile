FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

RUN --mount=type=secret,id=github_user \
    --mount=type=secret,id=github_token \
    dotnet nuget add source "https://nuget.pkg.github.com/lammeranssss/index.json" \
    -n "github" \
    -u "$(cat /run/secrets/github_user)" \
    -p "$(cat /run/secrets/github_token)" \
    --store-password-in-clear-text

ARG BUILD_CONFIGURATION=Release

COPY ["CarRental.API/CarRental.API.csproj", "CarRental.API/"]
COPY ["CarRental.BLL/CarRental.BLL.csproj", "CarRental.BLL/"]
COPY ["CarRental.DAL/CarRental.DAL.csproj", "CarRental.DAL/"]

RUN dotnet restore "./CarRental.API/CarRental.API.csproj"

COPY . .
WORKDIR "/src/CarRental.API"
RUN dotnet build "./CarRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRental.API.dll"]
