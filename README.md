# ToDo API

This is a simple ToDo API built with ASP.NET Core and PostgreSQL. It allows you to create, update, delete, fetch, and het list of tasks. The API is structured using MVC pattern and includes unit tests for the controller actions.

## Requirements

- .NET 8.0 or later
- PostgreSQL

## Setup and Installation

### 1. Clone the repository

First, clone the repository to your local machine


### 2. Set up the PostgreSQL Database
Make sure you have PostgreSQL installed and running on your machine.

Create a PostgreSQL database called todo_db (or another name if you prefer) by running the following SQL command in your PostgreSQL client:

**CREATE DATABASE todo_db;**


### 3. Configure the Connection String
Open the **appsettings.json** file and update the connection string under the ConnectionStrings section:


"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=todo_db;Username=postgres;Password=pass"
}

Replace "localhost", "postgres", and "pass" with your actual database details.


### 4. Apply Database Migrations
The API uses Entity Framework Core to manage the database schema. To apply the migrations and create the necessary tables, run the following command:

**dotnet ef database update**


### 5. Run the Application
Now that the database is set up, you can run the application:

**dotnet run**


### 6. API Endpoints
The following endpoints are available in the API:

- GET /api/todo: Retrieves all tasks.
- POST /api/todo: Creates a new task. Requires a valid task body.
- PUT /api/todo/{id}: Updates an existing task by its ID.
- DELETE /api/todo/{id}: Deletes a task by its ID.


### 7. Testing
The project includes unit tests to ensure the functionality works correctly. You can run the tests with the following command:

**dotnet test**

The tests verify the correct behavior of the API controller actions.
