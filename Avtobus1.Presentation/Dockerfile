FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Avtobus1.Presentation/Avtobus1.Presentation.csproj", "Avtobus1.Presentation/"]
COPY ["Avtobus1.Infrastructure/Avtobus1.Infrastructure.csproj", "Avtobus1.Infrastructure/"]
COPY ["Avtobus1.Application/Avtobus1.Application.csproj", "Avtobus1.Application/"]
COPY ["Avtobus1.Domain/Avtobus1.Domain.csproj", "Avtobus1.Domain/"]
RUN dotnet restore "Avtobus1.Presentation/Avtobus1.Presentation.csproj"
COPY . .
WORKDIR "/src/Avtobus1.Presentation"
RUN dotnet build "./Avtobus1.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Avtobus1.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avtobus1.Presentation.dll"]
