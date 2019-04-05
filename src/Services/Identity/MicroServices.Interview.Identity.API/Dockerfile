FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["src/Services/Identity/MicroServices.Interview.Identity.API/MicroServices.Interview.Identity.API.csproj", "src/Services/Identity/MicroServices.Interview.Identity.API/"]
RUN dotnet restore "src/Services/Identity/MicroServices.Interview.Identity.API/MicroServices.Interview.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/MicroServices.Interview.Identity.API"
RUN dotnet build "MicroServices.Interview.Identity.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroServices.Interview.Identity.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroServices.Interview.Identity.API.dll"]
