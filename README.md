**AbySalto – Technical Assignment**



Overview

This project is an ASP.NET Core Web API implementing
JWT authentication (Register  Login  Me)
Product integration via DummyJSON external API
Favorites functionality
Basket (cart) functionality
Entity Framework Core with SQL Server
Clean separation of concerns (Controllers - Stores - Infrastructure)
The solution follows a layered architecture approach.



Tech Stack
.NET 9
ASP.NET Core Web API
Entity Framework Core (SQL Server)
JWT Authentication
HttpClientFactory
Swagger (OpenAPI)
BCrypt for password hashing





**Architecture**


Layers
WebApi
Controllers (thin HTTP layer)
JWT configuration
Swagger configuration



Infrastructure
EF Core DbContext
Migrations
Stores (FavoritesStore, BasketStore)
External API integration (DummyJsonProductClient)



External
DummyJSON integration using HttpClientFactory



Design Decisions
Controllers contain only orchestration logic.
Database logic moved to Store classes.
External API logic isolated in dedicated client.
JWT authentication configured via middleware.

Passwords hashed using BCrypt.







**How to Run**
1️. Clone repository

2️. Ensure SQL Server is running



The project uses

Server=localhost;
Database=AbySaltoMidDb;
Trusted\_Connection=True;
TrustServerCertificate=True



If using Windows Authentication, no further setup is needed.



3️. Apply migrations
dotnet ef database update --project AbySalto.Mid.Infrastructure --startup-project AbySalto.Mid.WebApi



This will create the database automatically.



4️. Run the project
dotnet run --project AbySalto.Mid.WebApi



Open Swagger in browser
httpslocalhostxxxx









**Authentication Flow:**

1. Register user via:

    POST api/Auth/register



2. Login:
   POST api/Auth/login
3. Copy returned JWT token.
4. In Swagger, click Authorize and paste the token.
5. Access secured endpoints
   Favorites
   Basket
   api/Auth/me





**Features Implemented:**
Auth:
Register
Login (JWT)
Get current user (protected)



Products:
Get product by id
Get products with skiplimit (pagination)



Favorites:
Add to favorites
Remove from favorites
Get favorites (returns product details from DummyJSON)



Basket:
Add item (with quantity)
Remove item
Get basket (returns product details with quantity)











Notes
Passwords are securely hashed using BCrypt.
JWT key is stored in configuration for development purposes.
In production, it should be stored in environment variables or secret storage.
HttpClientFactory is used for external API integration.
Clean separation of DB logic and controller logic via Stores.

