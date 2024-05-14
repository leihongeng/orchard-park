#See https://aka.ms/containercompat for more information on Windows vs. Linux containers.

# 指定基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 构建阶段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orchard.Park.Management.API.csproj", "."]
RUN dotnet restore "./Orchard.Park.Management.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Orchard.Park.Management.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 发布阶段
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Orchard.Park.Management.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 最终镜像
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orchard.Park.Management.API.dll"]
