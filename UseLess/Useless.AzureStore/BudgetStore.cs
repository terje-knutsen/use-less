using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Useless.AzureStore
{
    internal class BudgetStore
    {
        private readonly CosmosClient client;
        public BudgetStore()
        {
            string endpoint = "https://localhost:8081/";
            string key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var options = new CosmosClientOptions() 
            {
                ConnectionMode = ConnectionMode.Direct,
                ConsistencyLevel = ConsistencyLevel.Eventual,
            };

            client = new(endpoint, key, options);
        }
        /// <summary>
        /// Id	            - Gets the unique name of the account
        /// ReadableRegions - Gets a list of readable locations for the account
        /// WritableRegions - Gets a list of writable locations for the account
        /// Consistency     - Gets the default consistency level for the account
        /// </summary>
        public Task<AccountProperties> AccountProperties => client.ReadAccountAsync();
        public Database GetDatabase(string name) => client.GetDatabase(name);
        public async Task<Database> CreateDatabase(string name) =>  await client.CreateDatabaseAsync(name);
        public async Task<Database> CreateIfNotExist(string name) => await client.CreateDatabaseIfNotExistsAsync(name);
        public Container GetContainer(Database database, string name) =>  database.GetContainer(name);
        public async Task<Container> CreateContainer(Database database, string dbName, string partitionKeyPath) 
           => await database.CreateContainerAsync(dbName,partitionKeyPath,400);
        public async Task<Container> CreateContainerIfNotExist(Database database, string dbName, string partitionKeyPath)
            => await database.CreateContainerIfNotExistsAsync(dbName, partitionKeyPath,400);
    }
}
