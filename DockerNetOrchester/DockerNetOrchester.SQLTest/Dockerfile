#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DockerNetOrchester.SQLTest/DockerNetOrchester.SQLTest.csproj", "DockerNetOrchester.SQLTest/"]
COPY ["DockerNetOrchester.Data/DockerNetOrchester.Data.csproj", "DockerNetOrchester.Data/"]
RUN dotnet restore "DockerNetOrchester.SQLTest/DockerNetOrchester.SQLTest.csproj"
COPY . .
WORKDIR "/src/DockerNetOrchester.SQLTest"
RUN dotnet build "DockerNetOrchester.SQLTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerNetOrchester.SQLTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerNetOrchester.SQLTest.dll"]