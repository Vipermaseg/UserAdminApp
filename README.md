# .NET, Angular 17 Example SPA Project with minimal APIs

## Overview

This project is an example of a full-stack application developed using .NET 8 with minimal APIs and ready to be deployed in containers to a productive environment.

## How to run

- **API:**
    - Open a terminal in `api/UserAdminApi/UserAdminApi` directory
    - Run the command: `dotnet run --launch-profile https`

- **API Tests:**
    - Open a terminal in `api/TestUserAdminApi/TestUserAdminApi` directory
    - Run the command: `dotnet test`

- **Front:**
    - Open a terminal in `front` directory
    - Run the command: `npm start`

## Technologies Used

- .NET 6.0
- EF Core
- SQLite
- Minimal APIs
- Fluent Validation
- Swagger
- Angular 17
- Angular Material
- Tailwind CSS

## Implementation details

In order to expedite the process, tests have been omitted from the front-end project. The repository and service layers of the back-end have been consolidated, which complicates testing. While this approach may be suitable for this particular project, it would not be ideal in a production development environment. In such a scenario, I would recommend separating the layers or utilizing a third-party library to enhance the structure.