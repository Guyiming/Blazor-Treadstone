#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["SysOps/OpsMain.IdsAuthentication/OpsMain.IdsAuthentication.csproj", "SysOps/OpsMain.IdsAuthentication/"]
COPY ["SysOps/OpsMain.StorageLayer/OpsMain.StorageLayer.csproj", "SysOps/OpsMain.StorageLayer/"]
COPY ["SysOps/OpsMain/Shared/OpsMain.Shared.csproj", "SysOps/OpsMain/Shared/"]
RUN dotnet restore "SysOps/OpsMain.IdsAuthentication/OpsMain.IdsAuthentication.csproj"
COPY . .
WORKDIR "/src/SysOps/OpsMain.IdsAuthentication"
RUN dotnet build "OpsMain.IdsAuthentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpsMain.IdsAuthentication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpsMain.IdsAuthentication.dll"]