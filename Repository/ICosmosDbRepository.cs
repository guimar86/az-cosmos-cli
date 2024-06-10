using cosmodb.Model;

namespace cosmodb.Repository;

public interface ICosmosDbRepository<T> where T : CosmosDocumentBase
{
    Task<T> GetItemAsync(string id);
    Task<IEnumerable<T>> GetItemsAsync();
    Task AddItemAsync(T item);
    Task UpdateItemAsync(string id, T item);
    Task DeleteItemAsync(string id);

    Task DeleteRangeAsync(List<T> items);
    Task DeleteRecordsByCustomerAsync(string customerId);
}