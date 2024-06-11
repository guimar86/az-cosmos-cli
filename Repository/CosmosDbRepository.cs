using cosmodb.Commands;
using cosmodb.Helpers;
using cosmodb.Model;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace cosmodb.Repository;

public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T : CosmosDocumentBase
{
    private readonly Container _container;

    public CosmosDbRepository()
    {
        _container = CosmosCommandBase.GetInstanceAsync().Result.Container;
    }

    public async Task<T> GetItemAsync(string id)
    {
        try
        {
            FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(
                queryDefinition: new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                    .WithParameter("@id", id),
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = null // Perform cross-partition query
                });

            if (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                return response.FirstOrDefault();
            }
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"Item with id: {id} not found");
            return null;
        }

        return null;
    }


    public async Task<IEnumerable<T>> GetItemsAsync()
    {
        string queryString = Common.MainSelect;
        var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
        List<T> results = new List<T>();
        while (query.HasMoreResults)
        {
            FeedResponse<T> response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }


    public async Task AddItemAsync(T item)
    {
        var response = await _container.CreateItemAsync(item, new PartitionKey(item.PartitionKey));
        Console.WriteLine(JsonConvert.SerializeObject(response.Resource));
    }

    public async Task AddItemAsync(T item, PartitionKey partitionKey)
    {
        var response = await _container.CreateItemAsync(item, partitionKey);
        Console.WriteLine(JsonConvert.SerializeObject(response.Resource));
    }

    public async Task AddItemAsync(T item, string partitionKey)
    {
        var response = await _container.CreateItemAsync(item, new PartitionKey(partitionKey));
        Console.WriteLine(JsonConvert.SerializeObject(response));
    }


    public async Task UpdateItemAsync(string id, T item)
    {
        await _container.UpsertItemAsync(item, new PartitionKey(id));
    }

    public async Task DeleteItemAsync(string id)
    {
        var item = await GetItemAsync(id);
        await _container.DeleteItemAsync<T>(item.Id, new PartitionKey(item.PartitionKey));
    }


    public async Task DeleteRangeAsync(List<T> items)
    {
        foreach (var item in items)
        {
            try
            {
                await _container.DeleteItemAsync<T>(item.Id, new PartitionKey(item.PartitionKey));
                Console.WriteLine($"Deleted document with ID: {item}");
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Document with ID {item} not found, skipping...");
            }
        }
    }

    public async Task DeleteRecordsByCustomerAsync(string customerId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.CustomerId = @CustomerId")
            .WithParameter("@CustomerId", customerId);

        await DeleteRecordsAsync(query);
    }

    private async Task DeleteRecordsAsync(QueryDefinition query)
    {
        var queryIterator = _container.GetItemQueryIterator<T>(query);

        var tasks = new List<Task>();

        while (queryIterator.HasMoreResults)
        {
            var response = await queryIterator.ReadNextAsync();

            foreach (var item in response)
            {
                var deleteTask = _container.DeleteItemAsync<T>(item.Id, new PartitionKey(item.PartitionKey));
                tasks.Add(deleteTask);
            }
        }

        await Task.WhenAll(tasks);
    }
}