#!/bin/sh

echo "Running EF Core migrations..."

dotnet ef database update \
    --project src/Core/Infrastructure/Infrastructure.csproj \
    --startup-project src/Web/Web.csproj

echo "Starting API.."

exec dotnet /app/publish/Web.dll