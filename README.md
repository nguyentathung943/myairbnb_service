# MyAirbnb API Service

## Features

- **Authentication**:  
  Default user credentials:  
  - **Id**: `1`  
  - **Username**: `admin`  
  - **Password**: `admin`

- **Accommodation Get List**:  
  Supports filtering by:  
  - Search string  
  - Capacity  
  - Check-in and check-out date range  
  - Offset and limit for pagination or infinite loading

- **Reservation Process**:  
  Book an accommodation with proper validation.

- **JWT Authentication**:  
  Secure access token management.

- **CORS Support**:  
  Cross-Origin Resource Sharing enabled.

- **Swagger Documentation**:  
  Automatically generated API documentation.


## Technologies Used

- **.NET Core 8**  
- **Entity Framework Core** (Migrations)  
- **PostgreSQL**  
- **JWT Authentication**  
- **Serilog**: For logging  
- **Docker Compose**: Includes pgAdmin & PostgreSQL  
- **Mapster**: For object mapping
- **ErrorOr**: Meaningful error handling objects  
- **MediatR**: Service request-response pattern  
- **MOQ, xUnit**: Unit testing for controllers and MediatR commands  

## How to run project
- Download Docker
- CD into MyAirbnb\MyAirbnb.DataAccess and run 

```bash
   docker-compose up
```

- account for loggin into pgAdmin in http://localhost:8080/ is 
   + Username: admin@admin.com
   + Password: admin
   + Hostname: Postgres
   + Database Port: 5432
   + Database User Name: admin
   + Database User Password: admin

- Open project, choose profile **MyAirbnb**
- Install EFCore tool in Nuget Package Manager Console: (https://learn.microsoft.com/en-us/ef/core/cli/powershell#install-the-tools)

```bash
   Install-Package Microsoft.EntityFrameworkCore.Tools 
```

- Choose the project MyAirbnb.DataAccess
- Perform the migration by

```bash
   Update-Database
```

- Run the project and access https://localhost:7039/index.html

### Prerequisites

- .NET Core SDK (.NET 8)
- Docker
- Visual Studio
