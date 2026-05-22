
# Project Setup

Here is a brief description of what this project is and some key notes on how to run the project in your local machines.

## Architecture Notes

The approach for this project had conperated concept of Clean Architecture like Layering of Applicaton, Domain, Infrastructure, Presentation. That promoted a better seperation of concern and it allow ease of Unit Test/Integration Test Implementation. Hence allow the code base to be cleaner, easier to scale and better readablility.

In the Application Layer of Clean Architecture, A CQRS design pattern had been added to allow a better seperation of concern of Command and Query.

## Prerequisition

Here is the list of Prerequisition that you need to do before running the Project in your PC

    1. Install DotNet 10 Sdk (I used 10.0.300 but any version of 10 should work)
    2. Install Docker Desktop (I used Docker version 29.4.3)
    3. Install IDE (I used Visual Studio Community Version 18.6.1)
## Run Locally

Step 1 : Clone the project

```bash
  git clone https://github.com/pangyeeshen23/webby-test.git
```
Step 2 : Open the project in IDE

Step 3 : Go to the project directory

```bash
  cd webby-test/App
```

Step 4 : Run Migrations

```bash
  dotnet ef database update --project src\Core\Infrastructure\Infrastructure.csproj --startup-project src\Web\Web.csproj
```

Step 5 : Run Docker Compose Up To Set Up MSSQL + Redis

```bash
  cd  webby-test/App/src/Web 
```

Step 6 : Run and Build the project using that IDE

Step 7 : Navigate To /swagger for REST API Document

## Migrations

This section would explain how to add migration.

Step 1 : Navigate To App

```bash
  cd webby-test/App
```

Step 2 : Open the project in IDE
```bash
  dotnet ef migrations add {ReplaceWithMigrationName} --project src\Core\Infrastructure\Infrastructure.csproj --startup-project src\Web\Web.csproj
```