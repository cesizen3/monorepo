# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers du projet
COPY . .

# Restaurer les dépendances
RUN dotnet restore

# Publier le projet en mode Release
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Démarrer l'application
ENTRYPOINT ["dotnet", "CESIZen.dll"]
EXPOSE 12000
