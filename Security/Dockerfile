FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Security/Security.csproj", "Security/"]
RUN dotnet restore "Security/Security.csproj"
COPY . .
WORKDIR "/src/Security"
RUN dotnet build "Security.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Security.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Security.dll"]
