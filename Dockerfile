# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el csproj y restaurar dependencias
COPY ["GestionCalidad.csproj", "./"]
RUN dotnet restore "./GestionCalidad.csproj"

# Copiar todo el proyecto
COPY . .

# Publicar en modo release
RUN dotnet publish "GestionCalidad.csproj" -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar publicación
COPY --from=build /app/publish .

# Render usa el puerto 10000
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Ejecutar API
ENTRYPOINT ["dotnet", "GestionCalidad.dll"]