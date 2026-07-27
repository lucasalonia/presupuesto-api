# --- Etapa de build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos primero el csproj para aprovechar la cache de capas en el restore
COPY presupuesto-api.csproj ./
RUN dotnet restore "presupuesto-api.csproj"

# Copiamos el resto del código y publicamos
COPY . .
RUN dotnet publish "presupuesto-api.csproj" -c Release -o /app/publish --no-restore

# --- Etapa de runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Usuario no root (la imagen aspnet de .NET ya trae el usuario "app")
USER app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "presupuesto-api.dll"]
