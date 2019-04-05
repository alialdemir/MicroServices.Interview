FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["MicroServices.Interview.OcelotApiGw/MicroServices.Interview.OcelotApiGw.csproj", "MicroServices.Interview.OcelotApiGw/"]
RUN dotnet restore "MicroServices.Interview.OcelotApiGw/MicroServices.Interview.OcelotApiGw.csproj"
COPY . .
WORKDIR "/src/MicroServices.Interview.OcelotApiGw"
RUN dotnet build "MicroServices.Interview.OcelotApiGw.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroServices.Interview.OcelotApiGw.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroServices.Interview.OcelotApiGw.dll"]