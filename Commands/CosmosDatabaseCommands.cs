using Cocona;
using cosmodb.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace cosmodb.Commands;

public class CosmosDatabaseCommands
{
    private readonly IDatabaseRepository _databaseRepository;
    private readonly ILogger<CosmosDatabaseCommands> _logger;

    public CosmosDatabaseCommands(IDatabaseRepository databaseRepository, ILogger<CosmosDatabaseCommands> logger)
    {
        _databaseRepository = databaseRepository;
        _logger = logger;
    }

    [Command("create", Description = "Create a new database. If it exists already, it won't be replicated")]
    public async Task<Database> CreateDatabase([Argument("databaseId", Description = "Database id or name")] string databaseId)
    {
        try
        {
            var database = await _databaseRepository.CreateDatabase(databaseId);
            if (database == null) return null;
            _logger.LogInformation("Database created");
            return database;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    [Command("find", Description = "Find database through its id or name")]
    public async Task<Database> FindDatabase([Argument("databaseId", Description = "Database id or name")] string databaseId)
    {
        try
        {
            var database = await _databaseRepository.FindDatabase(databaseId);
            if (database == null)
            {
                _logger.LogWarning("Database {id} was not found", databaseId);
                return null;
            }
            _logger.LogInformation("Database {id} was found", databaseId);
            return database;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
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