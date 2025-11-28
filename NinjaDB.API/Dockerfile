FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["NinjaDB.API/NinjaDB.API.csproj", "NinjaDB.API/"]
COPY ["NinjaDB/NinjaDB.csproj", "NinjaDB/"]

# Restore dependencies
RUN dotnet restore "NinjaDB.API/NinjaDB.API.csproj"

# Copy all source code
COPY . .

# Build the API project
WORKDIR "/src/NinjaDB.API"
RUN dotnet build "NinjaDB.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NinjaDB.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NinjaDB.API.dll"]