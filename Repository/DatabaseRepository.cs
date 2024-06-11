using System.Configuration;
using cosmodb.Commands;
using Microsoft.Azure.Cosmos;

namespace cosmodb.Repository;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosCommandBase _cosmosCommandBase;

    public DatabaseRepository()
    {
        _cosmosCommandBase = CosmosCommandBase.GetInstanceAsync().Result;
        _cosmosClient = _cosmosCommandBase.CosmosClient;
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
        await _cosmosCommandBase.ResetDatabaseAndContainerAsync();
    }
}