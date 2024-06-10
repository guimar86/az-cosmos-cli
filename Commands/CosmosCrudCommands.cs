using System.Text.Json;
using Cocona;
using cosmodb.Model;
using cosmodb.Repository;

namespace cosmodb.Commands;

public class CosmosCrudCommands
{
    private readonly ICosmosDbRepository<Document> _cosmosDbRepository;

    public CosmosCrudCommands(ICosmosDbRepository<Document> cosmosDbRepository)
    {
        _cosmosDbRepository = cosmosDbRepository;
    }

    [Command("list", Description = "List all documents in cosmos db")]
    public async Task<IEnumerable<Document>> List()
    {
        var result = await _cosmosDbRepository.GetItemsAsync();
        var documents = result.ToList();
        if (!documents.Any())
        {
            return null;
        }

        var formattedJson = JsonSerializer.Serialize(documents, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Console.WriteLine(formattedJson);
        Console.WriteLine($"Total number of results {documents.Count()}");
        return documents;
    }

    [Command("find", Description = "Find a specific document")]
    public async Task<Document> Find([Argument(Name = "id", Description = "id of document to find")] string id)
    {
        var document = await _cosmosDbRepository.GetItemAsync(id);
        if (document != null)
        {
            var formattedJson = JsonSerializer.Serialize(document, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            Console.WriteLine(formattedJson);
            return document;
        }

        Console.WriteLine("Document not found");
        return null;
    }

    [Command("delete", Description = "Delete a specific document")]
    public async Task Delete([Argument("id", Description = "id of document to delete")] string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        await _cosmosDbRepository.DeleteItemAsync(id);
    } 
    
    [Command("delete-range", Description = "Delete a specific document")]
    public async Task DeleteRange([Argument("items", Description = "list of items to delete")] List<Document> items)
    {

        await _cosmosDbRepository.DeleteRangeAsync(items);
    }

    [Command("delete-customer", Description = "Delete all documents from a specific cliente")]
    public async Task DeleteByCustomer([Argument("customerId", Description = "customer id to delete documents from")] string customerId)
    {
        await _cosmosDbRepository.DeleteRecordsByCustomerAsync(customerId);
    }
}