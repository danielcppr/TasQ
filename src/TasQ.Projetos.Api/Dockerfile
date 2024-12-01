# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TasQ.Projetos.Api/TasQ.Projetos.Api.csproj", "src/TasQ.Projetos.Api/"]
COPY ["src/TasQ.Projetos.Data/TasQ.Projetos.Data.csproj", "src/TasQ.Projetos.Data/"]
COPY ["src/TasQ.Core/TasQ.Core.csproj", "src/TasQ.Core/"]
COPY ["src/TasQ.Projetos.Domain/TasQ.Projetos.Domain.csproj", "src/TasQ.Projetos.Domain/"]
RUN dotnet restore "./src/TasQ.Projetos.Api/TasQ.Projetos.Api.csproj"
COPY . .
WORKDIR "/src/src/TasQ.Projetos.Api"
RUN dotnet build "./TasQ.Projetos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TasQ.Projetos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TasQ.Projetos.Api.dll"]