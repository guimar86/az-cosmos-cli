using System.Configuration;
using cosmodb.Commands;
using Microsoft.Azure.Cosmos;

namespace cosmodb.Repository;

public class DatabaseRepository : IDatabaseRepository
{
    private static readonly string EndpointUrl = ConfigurationManager.AppSettings["EndPointUri"];
    private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
    private CosmosClient _cosmosClient;

    public DatabaseRepository()
    {
        _cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
    }

    public async Task<Database> CreateDatabase(string databaseId)
    {
        return await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
    }

    public async Task<Database> FindDatabase(string databaseId)
    {
        try
        {
            var db = await _cosmosClient.GetDatabase(databaseId).ReadAsync();
            return db;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine(ex.Message);
            return null; // If NotFound, the database does not exist
        }
    }

    public async Task DeleteDatabase(string databaseId)
    {
        var db = await FindDatabase(databaseId);
        await db.DeleteAsync();
    }

    public async Task CleanAll()
    {
        var commandBase = await CosmosCommandBase.GetInstanceAsync();
        await commandBase.ResetDatabaseAndContainerAsync();
    }
}