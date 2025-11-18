# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG GITHUB_USER
ARG GITHUB_TOKEN
RUN dotnet nuget add source "https://nuget.pkg.github.com/lammeranssss/index.json" -n "github" -u $GITHUB_USER -p $GITHUB_TOKEN --store-password-in-clear-text
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarRental.API/CarRental.API.csproj", "CarRental.API/"]
COPY ["CarRental.BLL/CarRental.BLL.csproj", "CarRental.BLL/"]
COPY ["CarRental.DAL/CarRental.DAL.csproj", "CarRental.DAL/"]
RUN dotnet restore "./CarRental.API/CarRental.API.csproj"
COPY . .
WORKDIR "/src/CarRental.API"
RUN dotnet build "./CarRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRental.API.dll"]
