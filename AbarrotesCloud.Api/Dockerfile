# 1. Usar el SDK de .NET 8 para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["AbarrotesCloud.Api.csproj", "./"]
RUN dotnet restore "AbarrotesCloud.Api.csproj"

# Copiar todo lo demás y compilar
COPY . .
RUN dotnet publish "AbarrotesCloud.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Crear la imagen final súper ligera solo para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto que usa Render por defecto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "AbarrotesCloud.Api.dll"]