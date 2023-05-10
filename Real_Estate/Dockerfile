#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Real_Estate/Real_Estate.csproj", "Real_Estate/"]
RUN dotnet restore "Real_Estate/Real_Estate.csproj"
COPY . .
WORKDIR "/src/Real_Estate"
RUN dotnet build "Real_Estate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Real_Estate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Real_Estate.dll"]