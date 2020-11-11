#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DockerNetOrchester.LogWriter/DockerNetOrchester.LogWriter.csproj", "DockerNetOrchester.LogWriter/"]
RUN dotnet restore "DockerNetOrchester.LogWriter/DockerNetOrchester.LogWriter.csproj"
COPY . .
WORKDIR "/src/DockerNetOrchester.LogWriter"
RUN dotnet build "DockerNetOrchester.LogWriter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerNetOrchester.LogWriter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerNetOrchester.LogWriter.dll"]