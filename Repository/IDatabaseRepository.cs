using Microsoft.Azure.Cosmos;

namespace cosmodb.Repository;

public interface IDatabaseRepository
{
    Task<Database> CreateDatabase(string databaseId);
    Task<Database> FindDatabase(string databaseId);
    Task DeleteDatabase(string databaseId);

    Task CleanAll();
}