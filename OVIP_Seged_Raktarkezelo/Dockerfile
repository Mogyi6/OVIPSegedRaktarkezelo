# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# project fájl másolás + restore (gyors cache miatt)
COPY *.csproj ./
RUN dotnet restore

# teljes kód másolása
COPY . .

# publish
RUN dotnet publish -c Release -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# fontos Dockerhez
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

# ⚠️ EZT NE FELEJTSD EL ÁTÍRNI A SAJÁT DLL NEVRE
ENTRYPOINT ["dotnet", "OVIP_Seged_Raktarkezelo.dll"]