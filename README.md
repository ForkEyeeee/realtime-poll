
---

# Real-time Poll App

## Overview

Poll application built with ASP.NET CORE & PostgreSQL, featuring Google authentication, user-created polls, and real-time data display.

[🔴 Live](https://realtime-poll.onrender.com)
## Prerequisites

Before you start, ensure you have installed:
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started)

## Installation

### Localhost Development

1. **Clone the repository**

   ```bash
   git clone https://github.com/JBrown58/realtime-poll.git
   ```

2. **Install Dependencies**

   Navigate to the project directory and restore the .NET dependencies:

   ```bash
   dotnet restore
   ```

3. **Environment Setup**

   Create an `appsettings.json` file in the root of your project and enter your PostgreSQL connection string using the provided format. If you'd like to run the app with localhost only, you can leave DockerConnection blank.

   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "GoogleKeys": {
       "ClientId": "",
       "ClientSecret": ""
     },
     "ConnectionStrings": {
       "DevelopmentConnection": "Host=localhost;Port=5432;Database=realtime_poll;User Id=postgres;Password=<password>;",
       "DockerConnection": "Host=db;Port=5432;Database=realtime_poll;User Id=postgres;Password=<password>;",
     }
   }
   ```

   - **Note:** For `ClientId` and `ClientSecret` under `GoogleKeys`, enter your Google OAuth credentials.

5. **Run the Application**

   You can run the application using IIS Express or via the command line:

   ```bash
   dotnet run
   ```

   The app will be accessible at [https://localhost:44378](https://localhost:44378).

### Docker Setup

1. **Database Setup**

Certainly, let's correct the formatting for the database setup instructions:

1. **Database Setup**
   - Create a `db` directory at the root of your project.
   - Inside `db`, create a `password.txt` file containing your PostgreSQL password.
   - Create a `postgres.env` file at the root of your project.
   - `postgres.env`, should contain your database password in the following format:

     ```env
     POSTGRES_PASSWORD=<password>
     ```
2. **Docker Initialization**

      Before starting your containers, clean up any previous instances:

   ```bash
   docker-compose down -v
   ```

   To build and start the containers:

   ```bash
   docker-compose up --build
   ```

   If you want to watch for changes and automatically rebuild:

   ```bash
   docker-compose watch
   ```

3. **Access the Application**

   Once the Docker containers are up, the application will be accessible at [http://localhost:8080](http://localhost:8080).

## Dependencies

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.npgsql.org/efcore/)
- [Microsoft.AspNetCore.Authentication.Google](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-6.0)
- [SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-6.0)
- [xUnit](https://xunit.net/)
- [FakeItEasy](https://fakeiteasy.github.io/)
- [FluentAssertions](https://fluentassertions.com/)
- [Moq](https://github.com/moq/moq4)
- [Paginationjs](http://pagination.js.org/)
- [Docker](https://www.docker.com/get-started)

---