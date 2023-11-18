Introduction
The AmazonClone Web API facilitates CRUD (Create, Read, Update, Delete) operations for product management. It is built on ASP.NET Core and utilizes Entity Framework Core for data access. This API allows users to interact with the product database, performing operations like retrieving all products, retrieving a specific product by ID, deleting a product, and creating new products.

Endpoints
Get All Products
GET /Product/GetAllProducts
Description: Retrieves all products available in the database.
Response: Returns a list of all products.
Get Product by ID
GET /Product/GetByCode/{productId}
Description: Retrieves a specific product by its ID.
Parameters: productId - ID of the product.
Response: Returns the product details corresponding to the provided ID.
Delete Product
DELETE /Product/DeleteProduct/{productId}
Description: Deletes a product from the database.
Parameters: productId - ID of the product to delete.
Response: Returns true if the deletion is successful.
Create Product
POST /Product/Create
Description: Creates a new product in the database.
Request Body: Expects a JSON object representing the new product (ProductEntity).
Response: Returns true upon successful creation of the product.
Authentication and Authorization
This API currently doesn't enforce authentication or authorization for its endpoints. However, there's a commented-out [Authorize(Roles="Admin")] attribute in the code for potential future implementation. Authentication and authorization mechanisms can be integrated as needed to control access to the endpoints based on user roles and permissions.

Setup
To set up and run the AmazonClone Web API locally, follow these steps:

Clone this repository.
Ensure you have the necessary prerequisites installed (ASP.NET Core, Entity Framework Core, etc.).
Set up the database connection and configurations in appsettings.json.
Build and run the application.
Usage
Once the API is running locally or deployed to a server, you can interact with it using HTTP requests (e.g., using tools like Postman or cURL). Refer to the provided endpoints and their respective methods for performing CRUD operations on the products.

Dependencies
This project relies on the following major dependencies:

ASP.NET Core
Entity Framework Core
Serilog