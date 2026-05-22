
# Project Setup

Here is a brief description of what this project is and some key notes on how to run the project in your local machines.





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

Step 7 : Navigate To /swagger for API Document

