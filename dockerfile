# Usamos la imagen oficial SDK para construir la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos el archivo csproj y restauramos dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiamos el resto del código y publicamos la app en modo Release
COPY . ./
RUN dotnet publish -c Release -o out

# Usamos la imagen runtime para correr la app (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiamos los archivos publicados desde la etapa de build
COPY --from=build /app/out .

# Exponemos el puerto 80 (asegúrate que tu app escuche en este puerto)
EXPOSE 80

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "ClienteAPI.dll"]
