# Usa la imagen oficial del SDK de .NET para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establece el directorio de trabajo en la imagen
WORKDIR /app

# Instala paquetes adicionales de Debian (si es necesario)
RUN apt-get update && \
    apt-get install -y \
    vim \
    curl \
    git \
    # Agrega aquí otros paquetes que necesites
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Copia el archivo de solución y los archivos de proyecto al contenedor
COPY N5now.App.Permissions.Api/N5now.App.Permissions.Api.csproj N5now.App.Permissions.Api/
COPY N5now.App.Permissions.AutoMappers/N5now.App.Permissions.AutoMappers.csproj N5now.App.Permissions.AutoMappers/
COPY N5now.App.Permissions.Features/N5now.App.Permissions.Features.csproj N5now.App.Permissions.Features/
COPY N5now.App.Permissions.IOC/N5now.App.Permissions.IOC.csproj N5now.App.Permissions.IOC/
COPY N5now.App.Infrastructure.Elasticsearch/N5now.App.Infrastructure.Elasticsearch.csproj N5now.App.Infrastructure.Elasticsearch/
COPY N5now.App.Infrastructure.Kafka/N5now.App.Infrastructure.Kafka.csproj N5now.App.Infrastructure.Kafka/
COPY N5now.App.Permissions.DataLayer/N5now.App.Permissions.DataLayer.csproj N5now.App.Permissions.DataLayer/
COPY N5now.App.Permissions.Repository/N5now.App.Permissions.Repository.csproj N5now.App.Permissions.Repository/
COPY N5now.App.Permissions.Domain/N5now.App.Permissions.Domain.csproj N5now.App.Permissions.Domain/
COPY N5now.App.Permissions.Common/N5now.App.Permissions.Common.csproj N5now.App.Permissions.Common/

# Restaura las dependencias del proyecto
RUN dotnet restore N5now.App.Permissions.Api/N5now.App.Permissions.Api.csproj

# Copia el resto de los archivos de la aplicación al contenedor
COPY . .

# Verifica el contenido del directorio de trabajo después de copiar todo
RUN echo "Contenido del directorio /app después de copiar todo:" && ls -R /app

# Compila la aplicación
RUN dotnet build N5now.App.Permissions.Api/N5now.App.Permissions.Api.csproj -c Release -o out

# Usa la imagen del runtime de .NET para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Instala paquetes adicionales de Debian (si es necesario)
RUN apt-get update && \
    apt-get install -y \
    vim \
    curl \
    # Agrega aquí otros paquetes que necesites
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Copia los archivos compilados desde el contenedor de construcción
COPY --from=build /app/out .

# Expone el puerto en el que la aplicación escuchará
EXPOSE 82

# Define el comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "N5now.App.Permissions.Api.dll"]
