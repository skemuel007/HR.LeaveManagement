# Leave Management Application

Leave Management app with CQRS implementation using MediatR and Clean Code Architecture

# App Settings
## Database and Email Settings:


#### Enable Secret Storage

To use user secrets, run the following command in the project directory

```
dotnet user-secrets init
```

#### Set the Connection String

```
dotnet user-secrets set "ConnectionStrings:LeaveManagementConnectionString" ""
```

#### Set the Email String

```
dotnet user-secrets set "EmailSettings:ApiKey" ""

dotnet user-secrets set "EmailSettings:FromName" ""

dotnet user-secrets set "EmailSettings:FromAddress" ""

```

The code below is stored in <i>secrets.json</i> file. The commands above translates to what is shown below


```
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "EmailSettings": {
    "ApiKey": "",
    "FromName": "",
    "FromAddress": ""
  }
```

## Running Migration

```powershell
add-migration InitialCreation
```

```powershell
update-database
```