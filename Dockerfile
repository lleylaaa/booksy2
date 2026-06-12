# syntax=docker/dockerfile:1

# ---- Build-fase: compileer en publiceer de app met de .NET 9 SDK ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopieer eerst de projectbestanden en herstel de NuGet-pakketten.
# Zo blijft deze laag in de cache zolang er geen .csproj wijzigt.
COPY Booksy/Booksy.csproj Booksy/
COPY DAL/DAL.csproj DAL/
COPY Interface/Interface.csproj Interface/
COPY ServiceLibrary/ServiceLibrary.csproj ServiceLibrary/
RUN dotnet restore Booksy/Booksy.csproj

# Kopieer de rest van de code en publiceer de webapplicatie.
COPY . .
RUN dotnet publish Booksy/Booksy.csproj -c Release -o /app --no-restore

# ---- Runtime-fase: draai de gepubliceerde app op de kleinere ASP.NET-image ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Dokploy/Traefik verwachten dat de container op poort 8080 luistert.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Booksy.dll"]
