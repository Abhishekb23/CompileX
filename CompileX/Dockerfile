# ============================
# BUILD STAGE
# ============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CompileX/CompileX.csproj CompileX/
RUN dotnet restore CompileX/CompileX.csproj

COPY . .
WORKDIR /src/CompileX
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ============================
# RUNTIME STAGE
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Install Java + Python
RUN apt-get update && \
    apt-get install -y openjdk-17-jdk python3 && \
    rm -rf /var/lib/apt/lists/*

# Copy app
COPY --from=build /app/publish .

# Copy runner scripts
COPY runners /runners
RUN chmod +x /runners/java/run.sh /runners/python/run.sh

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CompileX.dll"]
