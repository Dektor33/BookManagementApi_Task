# Book Management API

## Overview
The **Book Management API** is a C#-based RESTful API built using ASP.NET Core with Clean Architecture principles. It provides functionalities for managing books and user authentication using JWT tokens.

## Features
- User authentication with JWT-based token system
- Role-based authorization (Admin, Reader)
- CRUD operations for books
- Bulk operations for adding and deleting books
- Refresh token mechanism for extended authentication

## Technologies Used
- **ASP.NET Core** (Web API)
- **Entity Framework Core** (Database ORM)
- **JWT Authentication** (Secure authentication mechanism)
- **Microsoft SQL Server 2022 (Default database provider)

##Including all features: Using Popularity Score, From the
most popular to less popular with pagination  AND other key points of Task

## Default Users & Roles
The application seeds the database with the following default users and roles:

| Username | Password  | Role  |
|----------|----------|-------|
| admin    | Admin123 | Admin |
| reader   | Reader123 | Reader |

## Endpoints
### Authentication
- **POST** `/api/Auth/login` - Authenticate user and return JWT token
- **GET** `/api/Auth/loginByRefreshToken?refreshToken={token}` - Refresh expired token

### Books
- **GET** `/api/Books/titles?pageNumber={page}&pageSize={size}` - Get paginated book titles
- **GET** `/api/Books/{id}` - Get book by ID
- **POST** `/api/Books` - Add a new book (Admin only)
- **POST** `/api/Books/bulk` - Add multiple books (Admin only)
- **PUT** `/api/Books/{id}` - Update book details
- **DELETE** `/api/Books/{id}` - Delete a book
- **DELETE** `/api/Books/bulk` - Delete multiple books

## Setup Instructions
1. Clone the repository:
   ```bash
   git clone <repository-url>
   
2. Navigate to the project directory:BookManagementApi_GO

3. Install dependencies:
4. Update the database:

5. Run the application:


## Configuration
- Update **appsettings.json** with JWT secrets:
  ```json
  "Jwt": {
    "Secret": "YourJWTSecretKey",
    "RefreshTokenSecret": "YourRefreshTokenSecretKey"
  }
  ```
- Modify the database connection string as needed.

## License -Dektor33

