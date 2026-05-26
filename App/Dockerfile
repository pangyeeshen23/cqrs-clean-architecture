FROM mcr.microsoft.com/dotnet/sdk:10.0 

WORKDIR /app

COPY . .

RUN dotnet tool install --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet restore src/Web/Web.csproj

RUN dotnet publish src/Web/Web.csproj \
    -c Release \
    -o /app/publish


COPY entrypoint.sh .

RUN chmod +x entrypoint.sh

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["./entrypoint.sh"]