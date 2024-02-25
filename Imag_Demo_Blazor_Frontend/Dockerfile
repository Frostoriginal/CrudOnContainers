#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Imag_Demo_Blazor_Frontend/Imag_Demo_Blazor_Frontend.csproj", "Imag_Demo_Blazor_Frontend/"]
COPY ["Imag_Demo_Common_DataContext_SQLSERVER/Imag_Demo_Common_DataContext_SQLSERVER.csproj", "Imag_Demo_Common_DataContext_SQLSERVER/"]
COPY ["Imag_Demo_Common_EntityModels_SQLSERVER/Imag_Demo_Common_EntityModels_SQLSERVER.csproj", "Imag_Demo_Common_EntityModels_SQLSERVER/"]
RUN dotnet restore "./Imag_Demo_Blazor_Frontend/./Imag_Demo_Blazor_Frontend.csproj"
COPY . .
WORKDIR "/src/Imag_Demo_Blazor_Frontend"
RUN dotnet build "./Imag_Demo_Blazor_Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Imag_Demo_Blazor_Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imag_Demo_Blazor_Frontend.dll"]