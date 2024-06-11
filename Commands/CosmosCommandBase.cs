using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace cosmodb.Commands
{
    public class CosmosCommandBase
    {
        private static CosmosCommandBase _instance;
        public Container Container { get; private set; }
        private Database Database { get; set; }
        private static readonly string EndpointUrl = ConfigurationManager.AppSettings["EndPointUri"];
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["DatabaseId"];
        private static readonly string ContainerId = ConfigurationManager.AppSettings["ContainerId"];
        public CosmosClient CosmosClient { get; set; }

        private CosmosCommandBase()
        {
        }

        public static async Task<CosmosCommandBase> GetInstanceAsync()
        {
            if (_instance == null)
            {
                _instance = new CosmosCommandBase();
                await _instance.InitCosmos();
            }

            return _instance;
        }

        public async Task ResetDatabaseAndContainerAsync()
        {
            // Delete the current container if it exists
            if (Container != null)
            {
                await Container.DeleteContainerAsync();
            }

            // Delete the current database if it exists
            if (Database != null)
            {
                await Database.DeleteAsync();
            }

            // Recreate the database and container
            await InitCosmos();
        }

        private async Task InitCosmos()
        {
            CosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);

            // Create a database if it doesn't exist
            Database = await CosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);

            var subPartitionKeys = new List<string>()
            {
                "/customerId"
            };

            var containerProperties = new ContainerProperties(id: ContainerId, partitionKeyPaths: subPartitionKeys);

            Container = await Database.CreateContainerIfNotExistsAsync(containerProperties);
            Console.WriteLine("Container started");
        }
    }
}