FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FuStudy_API/FuStudy_API.csproj", "FuStudy_API/"]
COPY ["FuStudy_Model/FuStudy_Model.csproj", "FuStudy_Model/"]
COPY ["FuStudy_Repository/FuStudy_Repository.csproj", "FuStudy_Repository/"]
COPY ["FuStudy_Service/FuStudy_Service.csproj", "FuStudy_Service/"]
COPY ["Tools/Tools.csproj", "Tools/"]
RUN dotnet restore "FuStudy_API/FuStudy_API.csproj"
COPY . .
WORKDIR "FuStudy_API/"
RUN dotnet build "FuStudy_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FuStudy_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS="http://+:8080"
ENTRYPOINT ["dotnet", "FuStudy_API.dll"]