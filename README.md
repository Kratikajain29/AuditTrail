# Audit Trail API
A .NET 7 Web API for tracking entity changes with field-level auditing.

## Features
- Track entity changes (Create, Update, Delete)
- Automatic field-level change detection
- Query audit trails by entity, user, and date range
- SQLite database with Entity Framework Core
- Api Versioning
- Configuration validation
- Error handling and logging

## Prerequisites
- .NET 7.0 SDK or later
- Any code editor (Visual Studio, VS Code, etc.)

## Installation & Setup

1. **Clone or download the project**
   ```bash
   git clone https://github.com/Kratikajain29/AuditTrail.git
   cd AuditTrailAPI1
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

## Running the Application

### Option 1: Using dotnet CLI
```bash
dotnet run
```

### Option 2: Using Visual Studio
1. Open `AuditTrailAPI1.sln` in Visual Studio
2. Press `F5` or click "Start Debugging"

### Option 3: Using VS Code
1. Open the project folder in VS Code
2. Press `F5` or use the Run and Debug panel

## Accessing the API

Once running, the API will be available at:
- **Base URL**: `http://localhost:5066/swagger`
- **Health Check**: `http://localhost:5066/api/audittrail/health`

## API Endpoints

- `POST /api/audittrail` - Create audit trail
- `GET /api/audittrail/{entityName}` - Get audit trails
- `GET /api/audittrail/health` - Health check

## Testing the API

### Using curl

1. **Health Check**
   ```bash
   curl http://localhost:5066/api/audittrail/health
   ```

2. **Create Audit Trail**
   ```bash
   curl -X POST "http://localhost:5066/api/audittrail" \
     -H "Content-Type: application/json" \
     -d '{
       "beforeObject": null,
       "afterObject": {"id": 1, "name": "John Doe", "email": "john@example.com"},
       "entityName": "User",
       "userId": "admin123",
       "action": 0
     }'
   ```

3. **Get Audit Trails**
   ```bash
   curl "http://localhost:5066/api/audittrail/User"
   ```

### Using the provided test file
The project includes `test-examples.http` with ready-to-use test requests.

## Architecture

- **Controllers**: Handle HTTP requests and responses
- **Services**: Business logic and data processing
- **Models**: Data transfer objects and entities
- **Data**: Entity Framework Core with SQLite
- **Configuration**: Validated settings with dependency injection

## Development

### Project Structure
```
AuditTrailAPI1/
├── Controllers/          # API endpoints
├── Services/            # Business logic
├── Models/              # Data models
├── Data/                # Database context
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```
