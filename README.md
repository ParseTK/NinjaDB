# NinjaDB - Ledger Management System

A console-based CRUD application for managing customers, products, and orders using Entity Framework Core and SQL Server.

## Prerequisites

- .NET 8.0 SDK
- SQL Server (local or remote instance)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd NinjaDB
```

### 2. Configure Connection String

The connection string is stored securely using .NET User Secrets. Set it up with:

```bash
cd NinjaDB
dotnet user-secrets set "ConnectionStrings:NinjaLedgerDB" "Server=YOUR_SERVER;Database=NinjaLedgerDB;Trusted_Connection=True;Encrypt=False;"
```

**Replace `YOUR_SERVER` with your SQL Server instance name** (e.g., `localhost\SQLEXPRESS` or `DESKTOP-ABC\MSSQLSERVER01`)

### 3. Restore Packages

```bash
dotnet restore
```

### 4. Create the Database

Run the SQL script to create the database:

```sql
CREATE DATABASE NinjaLedgerDB;
GO

USE NinjaLedgerDB;
GO

-- Run your table creation scripts here
```

Or use Entity Framework migrations if you have them set up.

### 5. Run the Application

```bash
dotnet run
```

## Project Structure

```
NinjaDB/
├── Data/
│   └── NinjaLedgerDbContext.cs    # Database context
├── Interfaces/
│   ├── ICustomerService.cs
│   ├── IOrderService.cs
│   └── IProductService.cs
├── Models/
│   ├── Customers.cs
│   ├── Orders.cs
│   └── Products.cs
├── Services/
│   ├── CustomerService.cs
│   ├── OrderService.cs
│   └── ProductService.cs
└── Program.cs                      # Entry point with console UI
```

## Features

- **Customer Management**: Create, view, update, and delete customer records
- **Product Management**: Manage product catalog with pricing
- **Order Management**: Track customer orders with product quantities
- **Console UI**: Simple menu-driven interface for all operations

## Security Notes

- Connection strings are stored in User Secrets (not in source control)
- For production deployment, use environment variables or Azure App Configuration
- Never commit connection strings to Git

## Testing

The project includes a test suite. To run tests:

```bash
cd NinjaLedgerDB_Test
dotnet test
```

## Deployment Options

### Option 1: Azure App Service (Free Tier)
- Push to GitHub
- Create Azure App Service
- Configure connection string in Azure Portal → Configuration

### Option 2: Docker
- See `docker-compose.yml` for containerized deployment
- Use `.env` file (gitignored) for connection strings

## Contributing

1. Create a feature branch
2. Make your changes
3. Test thoroughly
4. Submit a pull request

