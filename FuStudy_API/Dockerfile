FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FUStudy_API/FuStudy_API.csproj", "FUStudy_API/"]
COPY ["FuStudy_Model/FuStudy_Model.csproj", "FuStudy_Model/"]
COPY ["FUStudy_Repository/FuStudy_Repository.csproj", "FUStudy_Repository/"]
COPY ["Tools/Tools.csproj", "Tools/"]
COPY ["FuStudy_Service/FuStudy_Service.csproj", "FuStudy_Service/"]
RUN dotnet restore "FUStudy_API/FuStudy_API.csproj"
COPY . .
WORKDIR "/src/FUStudy_API"
RUN dotnet build "FuStudy_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FuStudy_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FuStudy_API.dll"]
