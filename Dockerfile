# 指定基础镜像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 复制项目文件并还原依赖项
COPY . .

# 构建项目
RUN dotnet build "Orchard.Park.Management.API.csproj" -c Release -o /app/build

# 发布项目
FROM build AS publish
RUN dotnet publish "Orchard.Park.Management.API.csproj" -c Release -o /app/publish

# 最终镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orchard.Park.Management.API.dll"]
