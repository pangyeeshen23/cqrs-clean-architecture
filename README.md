
# Project Setup

Here is a brief description of what this project is and some key notes on how to run the project in your local machines.

## Architecture Notes

The approach for this project had conperated concept of Clean Architecture like Layering of Applicaton, Domain, Infrastructure, Presentation. Hence allow the code base to be cleaner, easier to scale and better readablility. That allows me to easily contiue with Unit Test/Integration Test Implementation. 

In the Application Layer of Clean Architecture, A CQRS design pattern had been added to allow a better seperation of concern of Command and Query.

## Prerequisition

Here is the list of Prerequisition that you need to do before running the Project in your PC

    1. Install DotNet 10 Sdk (I used 10.0.300 but any version of 10 should work)
    2. Install Docker Desktop (I used Docker version 29.4.3)
    3. Install IDE (I used Visual Studio Community Version 18.6.1)

## Run Locally - Docker Compose

Step 1 : Clone the project

```bash
  git clone https://github.com/pangyeeshen23/cqrs-clean-architecture.git
```
Step 2 : Open the project in IDE

Step 3 : Go to the project directory using the IDE's terminal

```bash
  cd cqrs-clean-architecture/App
```

Step 4 : Update the following file's credentials and rename them to the correct format

```bash
  /App/docker-compose-example.yml -> /App/docker-compose.yml
  Inside  /App/docker-compose.yml Replace All the YourPassword

  /App/Web/appsettings.Example.json -> /App/Web/appsettings.Development.json
  Inside appsettings.Development.json Update the Connection string to the credential that you had in the Dockerfile
```

Step 5 : Run Docker Compose Up To Set Up Api + MSSQL + Redis in the IDE's terminal

```bash
  docker compose up
```

Step 6 : Navigate to http://localhost:8080/swagger to view all endpoints

## Run Locally - MSSQL, Redis Install Locally

Step 1 : Clone the project

```bash
  git clone https://github.com/pangyeeshen23/cqrs-clean-architecture.git
```
Step 2 : Open the project in IDE

Step 3 : Go to the project directory using the IDE's terminal

```bash
  cd cqrs-clean-architecture/App
```

Step 4 : Update the following file's credentials and rename them to the correct format

```bash
  /App/Web/appsettings.Example.json -> /App/Web/appsettings.Development.json
  Inside appsettings.Development.json Update the Connection string to the credential that you had
```

Step 5 : Run Migrations

```bash
  dotnet ef database update --project src\Core\Infrastructure\Infrastructure.csproj --startup-project src\Web\Web.csproj
```

Step 6 : Run and Build the project using that IDE

Step 7 : Navigate To  https://localhost:8080/swagger for REST API Document

Notes : Remember To Run dotnet format on \App to format the project before pushing changes to main


## Test Project Setup


Step 1 : Update the appsettings.Example.json

```bash
  /App/src/Test/appsettings.Example.json -> /App/src/Test/appsettings.json
  Update the credential to your liking
```

Step 2 : Do Ensure that the main project can be build first. Run the test in the IDE's Test Explorer. 

## Migrations

This section would explain how to add migration.

Step 1 : Navigate To App

```bash
  cd cqrs-clean-architecture/App
```

Step 2 : Run the command below
```bash
  dotnet ef migrations add {ReplaceWithMigrationName} --project src\Core\Infrastructure\Infrastructure.csproj --startup-project src\Web\Web.csproj
```

This section would explain how to remove a migration

Step 1 : Navigate To App
```bash
  cd cqrs-clean-architecture/App
```

Step 2 : Run the command below
```bash
   dotnet ef migrations remove --project src\Core\Infrastructure\Infrastructure.csproj --startup-project src\Web\Web.csproj
```


## Error Handling Strategry

A Global Exception Handler had been added to produce a well formatted error message.

The json response format would consist of 
1. title - title of the exception, etc : Bad Request
2. status - status code, etc : 400 
3. detail - message that explain the exception : One or more validation errors occurred.
4. instance - the instance called : POST /user/register
5. errors - an array that display a detail look of the exception (only for validation exception)

An Example of 'errors'
```bash
  "errors": [
        {
            "propertyName": "Username",
            "errorMessage": "Username is required",
            "attemptedValue": ""
        }
    ]
```

## Database Indexing

As of now, Indexing that are added are on the foreign key of every table to allow faster search and join operation. Which is added automatically.

In the User table, an index had been added to the Username column because the login commnad had used where search for that.

## Logging & Monitoring

The framework used for logging is called SeriLog

It would produce a log to the console and a file format

What are being logged are shown below :
1. Request (High Level Info)
2. Liscensing Info (Depend On Library)
3. Stack Trace of Exception
4. Response (High Level Info)

## Api Rate Limiting
A global rate limiting has been added to all the controller in the project.

The strategy for this rate limiting introduce a pool of 10 request per IP per seconds on each API.
This means that each IP would be able to call to an API for 10 times per second.
There is a queue limit of 5 as well.

So it would be 10 immediate API calls and 5 in queue.


## Caching

Data that are being cache are data like tag and post

The invalidation strategy implemented is to remove cache whenever there is a action of insertion of record, update of record or delete or record of the same entity.

## CI/CD

Currently, The pipeline will run thru these process

1. Verifying C# code formatting with dotnet format
2. Run automated testing 

