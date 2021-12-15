FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["school-rest-api/school-rest-api.csproj", "school-rest-api/"]
RUN dotnet restore "school-rest-api/school-rest-api.csproj"
COPY . .
WORKDIR "/src/school-rest-api"
RUN dotnet build "school-rest-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "school-rest-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "school-rest-api.dll"]