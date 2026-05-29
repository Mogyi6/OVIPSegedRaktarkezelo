# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy solution and project files (cache friendly)
# build context is repository root, so these relative paths exist
COPY *.sln ./
COPY Logic/*.csproj Logic/
COPY Models/*.csproj Models/
COPY Repository/*.csproj Repository/
COPY OVIP_Seged_Raktarkezelo/*.csproj OVIP_Seged_Raktarkezelo/

RUN dotnet restore

# copy remaining sources
COPY . .

# publish the specific project
RUN dotnet publish OVIP_Seged_Raktarkezelo/OVIP_Seged_Raktarkezelo.csproj -c Release -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# fontos Dockerhez
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

# ⚠️ EZT NE FELEJTSD EL ÁTÍRNI A SAJÁT DLL NEVRE
ENTRYPOINT ["dotnet", "OVIP_Seged_Raktarkezelo.dll"]