FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RRS.Api/RRS.Api.csproj", "RRS.Api/"]
COPY ["RRS.Application/RRS.Application.csproj", "RRS.Application/"]
COPY ["RRS.Core/RRS.Core.csproj", "RRS.Core/"]
COPY ["RRS.Infrastructure/RRS.Infrastructure.csproj", "RRS.Infrastructure/"]
RUN dotnet restore "./RRS.Api/./RRS.Api.csproj"
COPY . .
WORKDIR "/src/RRS.Api"
RUN dotnet build "./RRS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RRS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RRS.Api.dll"]