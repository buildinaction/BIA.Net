# Database Handler (Worker Feature)
This file explains what to use the the database handler feature in your V3 project.

## Prerequisite

### Knowledge to have:
* [SQL language](https://sql.sh/)

### Database:
* The project database should be SQL Server
* Broker should be enable on the project database
```SQL
ALTER DATABASE [YourProjectDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
ALTER DATABASE [YourProjectDatabase] SET ENABLE_BROKER;
ALTER DATABASE [YourProjectDatabase] SET MULTI_USER WITH ROLLBACK IMMEDIATE
```

## Overview
* The worker service run code when there is change on the database.
* A fine selection of the rows to track can be done with a SQL query.

## Activation
### Api: 
* bianetconfig.json
In the BIANet Section add:
```Json
    "WorkerFeatures": {
      "DatabaseHandler": {
        "Activate": true
      }
    },
```

## Usage
## Create the handler repositories:
Create a repository classe in the worker project in floder Features this classe inherit of DatabaseHandlerRepository.
Example from BIADemo:
```CSharp
namespace Safran.BIADemo.WorkerService.Features
{
    using System.Data.SqlClient;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using BIA.Net.Core.WorkerService.Features.HubForClients;
    using Microsoft.Extensions.Configuration;

    public class PlaneHandlerRepository : DatabaseHandlerRepository
    {
        public PlaneHandlerRepository(IConfiguration configuration)
            : base(
            configuration.GetConnectionString("BIADemoDatabase"),
            "SELECT RowVersion FROM [dbo].[Planes]",
            "" /*"SELECT TOP (1) [Id] FROM [dbo].[Planes] ORDER BY [RowVersion] DESC"*/,
            r => PlaneChange(r))
            { }

        public static void PlaneChange(SqlDataReader reader)
        {
            //int id = reader.GetInt32(0);

            _ = HubForClientsService.SendMessage("refresh-planes", "");
        }
    }
}
```
In the constructor base you specify the parameters:
- The connection string
- The SQL query to track change
- The SQL query to execute when a change appear (if empty it do not execute query)
- The callback fonction to execute when a change appear

In the callback function :
- you can read the result of the query passed in 3th parameters, by using the reader passed in parameter.
- If the 3th paramter of the base constructor is empty the reader parameter is null.

### Parameters those repositories
In startup.cs you should pass the list of all yours database handler repositories class in the function config.DatabaseHandler.Activate.
Example from BIADemo:
```CSharp
    // Begin BIA Standard service
    services.AddBiaWorkerFeatures( config =>
        {
            config.BiaNetSection = biaNetSection;
            config.DistributedCache.Activate(configuration.GetConnectionString("BIADemoDatabase"));
            if (biaNetSection.WorkerFeatures.DatabaseHandler.Activate)
            {
                config.DatabaseHandler.Activate(new List<DatabaseHandlerRepository>()
                {
                    new PlaneHandlerRepository(configuration),
                });
            }
        });

    // End BIA Standard service
```
