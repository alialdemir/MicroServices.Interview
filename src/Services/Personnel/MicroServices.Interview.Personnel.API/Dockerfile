FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["src/Services/Personnel/MicroServices.Interview.Personnel.API/MicroServices.Interview.Personnel.API.csproj", "src/Services/Personnel/MicroServices.Interview.Personnel.API/"]
RUN dotnet restore "src/Services/Personnel/MicroServices.Interview.Personnel.API/MicroServices.Interview.Personnel.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Personnel/MicroServices.Interview.Personnel.API"
RUN dotnet build "MicroServices.Interview.Personnel.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroServices.Interview.Personnel.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroServices.Interview.Personnel.API.dll"]
