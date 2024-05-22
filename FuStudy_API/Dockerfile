#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FuStudy_API/FuStudy_API.csproj", "FuStudy_API/"]
COPY ["FuStudy_Model/FuStudy_Model.csproj", "FuStudy_Model/"]
COPY ["FuStudy_Repository/FuStudy_Repository.csproj", "FuStudy_Repository/"]
COPY ["Tools/Tools.csproj", "Tools/"]
COPY ["FuStudy_Service/FuStudy_Service.csproj", "FuStudy_Service/"]
RUN dotnet restore "./FuStudy_API/FuStudy_API.csproj"
COPY . .
WORKDIR "/src/FuStudy_API"
RUN dotnet build "./FuStudy_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FuStudy_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FuStudy_API.dll"]