# GameStore API

REST API built with ASP.NET Core Minimal API to manage a video game catalog. This project includes JWT authentication, role-based authorization, persistence with Entity Framework Core and SQLite, and CRUD operations for games.

## Features

- ASP.NET Core Minimal API
- Entity Framework Core with SQLite
- JWT authentication
- Role-based authorization (`Admin` and `User`)
- Video game CRUD operations
- Public and protected endpoints
- Validation with Data Annotations
- EF Core migrations

## Technologies

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- Swagger / OpenAPI

## Main Endpoints

### Auth

- `POST /auth/register`
- `POST /auth/login`

### Games

- `GET /games`
- `GET /games/{id}`
- `POST /games` `Admin only`
- `PUT /games/{id}` `Admin only`
- `DELETE /games/{id}` `Admin only`

### Genres

- `GET /genres`

## Main Model

Each game includes:

- name
- description
- platforms
- stock
- genre
- price
- release date

## Authentication and Roles

The API uses JWT to authenticate users.

- The first registered user is assigned the `Admin` role
- All subsequent users are assigned the `User` role
- Only `Admin` users can create, update, and delete games
- Read-only endpoints are public

## How to Run the Project

1. Clone the repository
2. Navigate to the `GameStore.Api` project
3. Restore dependencies
4. Run the API

