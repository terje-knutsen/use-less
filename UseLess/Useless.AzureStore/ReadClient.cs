using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Useless.AzureStore
{
    public sealed class ReadClient
    {
       const string endpoint = "https://localhost:8081/";
       const string key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
       static CosmosClientOptions options = new CosmosClientOptions()
        {
            ConnectionMode = ConnectionMode.Direct,
                ConsistencyLevel = ConsistencyLevel.Eventual,
        };
        private static readonly CosmosClient cosmosClient = new CosmosClient(endpoint,key,options);

        static ReadClient() { }
        private ReadClient() { }
        public static CosmosClient Instance => cosmosClient;
    }
}
