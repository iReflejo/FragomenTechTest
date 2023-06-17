FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FragomentTechTest.Api/FragomentTechTest.Api.csproj", "FragomentTechTest.Api/"]
RUN dotnet restore "FragomentTechTest.Api/FragomentTechTest.Api.csproj"
COPY . .
WORKDIR "/src/FragomentTechTest.Api"
RUN dotnet build "FragomentTechTest.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FragomentTechTest.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FragomentTechTest.Api.dll"]
