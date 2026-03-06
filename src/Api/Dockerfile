# ============================
# Etapa 1: BUILD (SDK .NET 10)
# ============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Api.csproj .
RUN dotnet restore Api.csproj
COPY . .

# Publicación self-contained (ajusta a linux-arm64 si tu host/CI es ARM)

RUN dotnet publish Api.csproj -c Release -o /app/publish \
    -r linux-x64 --self-contained true \
    /p:PublishReadyToRun=true


# ============================
# Etapa 2: RUNTIME (Debian 13)
# ============================
FROM debian:13-slim AS runtime

ENV DEBIAN_FRONTEND=noninteractive

# 👇 Paquetes para runtime .NET self-contained en Trixie:
# - libicu76 (ICU de Trixie)
# - libgcc-s1 (sustituye libgcc1)
# - libstdc++6, libc6, ca-certificates, tzdata, libgssapi-krb5-2
RUN apt-get update && apt-get install -y --no-install-recommends \
      ca-certificates tzdata libc6 libgcc-s1 libstdc++6 libgssapi-krb5-2 libicu76 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
# Con ICU instalado, mantenemos globalización completa
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

COPY --from=build /app/publish ./

# Cambia "Api" por el nombre real del ejecutable si tu ensamblado se llama distinto
ENTRYPOINT ["./Api"]