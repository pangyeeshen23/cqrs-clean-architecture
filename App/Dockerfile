FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Core/Application/*.csproj src/Core/Application/
COPY src/Core/Domain/*.csproj src/Core/Domain/
COPY src/Core/Infrastructure/*.csproj src/Core/Infrastructure/
COPY src/Core/Presentation/*.csproj src/Core/Presentation/
COPY src/Web/*.csproj src/Web/

RUN dotnet restore src/Web/Web.csproj

COPY . .

RUN dotnet publish src/Web/Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Web.dll"]