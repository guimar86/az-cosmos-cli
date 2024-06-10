using Cocona;
using cosmodb.Repository;
using Microsoft.Azure.Cosmos;

namespace cosmodb.Commands;

public class CosmosDatabaseCommands
{
    private readonly IDatabaseRepository _databaseRepository;

    public CosmosDatabaseCommands(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [Command("create", Description = "Create a new database. If it exists already, it won't be replicated")]
    public async Task<Database> CreateDatabase([Argument("databaseId", Description = "Database id or name")] string databaseId)
    {
        return await _databaseRepository.CreateDatabase(databaseId);
    }

    [Command("find", Description = "Find database through its id or name")]
    public async Task<Database> FindDatabase([Argument("databaseId", Description = "Database id or name")] string databaseId)
    {
        return await _databaseRepository.FindDatabase(databaseId);
    }

    [Command("delete", Description = "Delete database through its id or name")]
    public async Task DeleteDatabase([Argument("databaseId", Description = "Database id or name")] string databaseId)
    {
        await _databaseRepository.DeleteDatabase(databaseId);
    }

    [Command("clean", Description = "Find database through its id or name")]
    public async Task CleanDatabaseAndContainer()
    {
        await _databaseRepository.CleanAll();
    }
}