FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/ExperienceService.Core/ExperienceService.Core.csproj src/ExperienceService.Core/
COPY src/ExperienceService.Infrastructure/ExperienceService.Infrastructure.csproj src/ExperienceService.Infrastructure/
COPY src/ExperienceService.Api/ExperienceService.Api.csproj src/ExperienceService.Api/
RUN dotnet restore src/ExperienceService.Api/ExperienceService.Api.csproj

COPY src/ src/
RUN dotnet publish src/ExperienceService.Api/ExperienceService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ExperienceService.Api.dll"]
