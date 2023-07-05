# Task 2 - Account Receivable System
This solution consists of three services: IdentityServer, Server, and a client React app. The backend is developed using .NET 7 and MongoDB is used as the database.

## Definition
Task 2 - Account receivable system: you are building part of an accounts receivable system that should allow users to do following:
Create new invoices with the following fields:
* Customer Name
* Invoice Number
* Invoice Date
* Due Date
* Line Items (description, quantity, total price)
* View a list of all invoices sorted by due date.
* Mark invoices as paid.
* View a list of unpaid invoices.
* Calculate the total amount of unpaid invoices.
* Calculate the average time it takes an invoice to be paid. (from invoice date to paid date).

## Services

### IdentityServer

The IdentityServer service is a simplified Single Sign-On (SSO) service implemented in .NET 7. It provides registration and login functionality for users. The service ensures that each user can only access their own invoices. The service includes basic password hashing and validation, such as unique email addresses and matching passwords during login.

### Server

The Server service is the Account Receivable System service implemented in .NET 7. It contains integration tests for each endpoint, including creating invoices, updating invoices to set them as paid, and retrieving invoices in ascending order. Additionally, there are some unit tests implemented. The Server service follows the clean architecture design, with use cases for each business flow. It also includes basic validation rules, such as:

* Non-empty InvoiceNumber, CustomerName, and LineItems.
* Non-empty Description for each line item.
* Positive TotalPrice and Quantity for line items.
* Due Date validation: greater than or equal to the InvoiceDate and at least 6 hours in the future from the current date.
* Unique index in the database for the combination of CustomerName, InvoiceNumber, and InvoiceDate.

### Client App

The client app is implemented using React and styled using React Bootstrap. There is one protected route for the invoice dashboard, which can only be accessed after successful registration and login. The user's token is stored in the local storage, which is a basic implementation for simplicity but not secure. The client app provides the following functionality:

* Registration and login actions
* Navigation link to the invoices dashboard
* Adding new invoices
* Viewing all invoices
* Filtering invoices by the "is paid" property
* Setting invoices as paid

**Please note**: After completing the registration process, users are required to log in to the system.

## Issues and Future Improvements

While the solution provides the basic functionality, there are a few known issues and areas for improvement:

1. Rerendering of the dashboard component: The dashboard component is being rerendered multiple times, resulting in unnecessary retrieval of invoices from the backend. This can be optimized to reduce redundant API calls.
2. Lack of tests for the client app: Currently, there are no tests implemented for the client app. Adding tests for the client app would improve the overall quality and reliability of the solution.
3. Token storage: The token is stored in the local storage, which is not a secure approach. Implementing a more secure token management mechanism, such as using HttpOnly cookies, would enhance the security of the solution.

## Local Deployment with Docker Compose

To run the solution locally, follow these steps:

1. Ensure Docker and Docker Compose are installed on your local machine.
2. Navigate to the folder containing the **`docker-compose.yml`** file.
3. Run the command **`docker-compose up`** to start the deployment.
4. Access the UI by opening your browser and navigating to **`http://localhost:8087`**. The IdentityServer service will be available on port 8085, and the Server service on port 8086.

**Note**: The deployment requires Docker and Docker Compose to be installed and running on your local machine.

Please note that this solution provides the basic implementation for the given requirements and architecture. Additional improvements, such as further validation, security enhancements, and testing, can be added based on specific project requirements and considerations.