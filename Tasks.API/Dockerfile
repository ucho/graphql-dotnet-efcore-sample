FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY Tasks.API/*.csproj ./Tasks.API/
COPY Tasks.Domain/*.csproj ./Tasks.Domain/
COPY Tasks.Infrastructure/*.csproj ./Tasks.Infrastructure/ 
RUN dotnet restore Tasks.API/Tasks.API.csproj
COPY . .
WORKDIR /src/Tasks.API
RUN dotnet build Tasks.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tasks.API.dll"]