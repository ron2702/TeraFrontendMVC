# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar las dependencias
COPY ./TeraFrontendMVC/TeraFrontendMVC.csproj ./TeraFrontendMVC/
RUN dotnet restore ./TeraFrontendMVC/TeraFrontendMVC.csproj

# Copiar todo el contenido del proyecto y compilar
COPY ./TeraFrontendMVC/. ./TeraFrontendMVC/
WORKDIR /src/TeraFrontendMVC
RUN dotnet build -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagen final que se ejecutará
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish ./

# Establecer el puerto en el que correrá la aplicación
EXPOSE 80

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "TeraFrontendMVC.dll"]
