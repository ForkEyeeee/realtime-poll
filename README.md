# Real-time Poll App

Poll application built with ASP.NET CORE & PostgreSQL, featuring Google authentication, user-created polls, and real-time data display.

## Prerequisites

Before you begin, ensure you have the following installed:

- [Docker](https://www.docker.com/get-started)
- [PostgreSQL](https://www.postgresql.org/download/)

## Setup Instructions

Follow these steps to get the application up and running:

### 1. Clone the Repository

Start by cloning the repository to your local machine. Open a terminal and run:

```bash
git clone https://github.com/JBrown58/realtime-poll.git
```

### 2. Set Up Google OAuth Credentials

For user authentication, you'll need to obtain a Google Client ID and Client Secret. Here's how:

1. Visit the [Google Developer Console](https://console.developers.google.com/).
2. Create a project and navigate to the Credentials page.
3. Create OAuth 2.0 credentials to get your Client ID and Client Secret.

After getting the `ClientID` and `ClientSecret`, store them in your project:

```bash
dotnet user-secrets set "GoogleKeys:ClientId" "<ClientID>"
dotnet user-secrets set "GoogleKeys:ClientSecret" "<ClientSecret>"
```

### 3. Run with Docker

Navigate to the project root directory in your terminal, then run:

```bash
docker-compose up
```

Visit [https://localhost:8081](https://localhost:8081) to access the application.
