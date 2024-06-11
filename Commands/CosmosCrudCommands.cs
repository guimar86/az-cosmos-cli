using System.Text.Json;
using Cocona;
using cosmodb.Model;
using cosmodb.Repository;
using Microsoft.Extensions.Logging;

namespace cosmodb.Commands;

public class CosmosCrudCommands
{
    private readonly ICosmosDbRepository<Document> _cosmosDbRepository;
    private readonly ILogger<CosmosCrudCommands> _logger;

    public CosmosCrudCommands(ICosmosDbRepository<Document> cosmosDbRepository, ILogger<CosmosCrudCommands> logger)
    {
        _cosmosDbRepository = cosmosDbRepository;
        _logger = logger;
    }

    [Command("list", Description = "List all documents in cosmos db")]
    public async Task<IEnumerable<Document>> List()
    {
        var result = await _cosmosDbRepository.GetItemsAsync();
        var documents = result.ToList();
        if (!documents.Any())
        {
            _logger.LogWarning("No documents present");
            return null;
        }

        var formattedJson = JsonSerializer.Serialize(documents, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        _logger.LogInformation(formattedJson);
        _logger.LogInformation($"Total number of results {documents.Count()}");
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
          
            _logger.LogInformation(formattedJson);
            _logger.LogInformation("Record has been found for id {id}", id);
            return document;
        }

        _logger.LogWarning("Document not found");
        return null;
    }

    [Command("delete", Description = "Delete a specific document")]
    public async Task Delete([Argument("id", Description = "id of document to delete")] string id)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(id);
            await _cosmosDbRepository.DeleteItemAsync(id);
            _logger.LogInformation("Document for id {id} has been delted successfully", id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    [Command("delete-range", Description = "Delete a specific document")]
    public async Task DeleteRange([Argument("items", Description = "list of items to delete")] List<Document> items)
    {
        try
        {
            await _cosmosDbRepository.DeleteRangeAsync(items);
            _logger.LogInformation("Logs in the specified range have been deleted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    [Command("delete-customer", Description = "Delete all documents from a specific cliente")]
    public async Task DeleteByCustomer([Argument("customerId", Description = "customer id to delete documents from")] string customerId)
    {
        try
        {
            await _cosmosDbRepository.DeleteRecordsByCustomerAsync(customerId);
            _logger.LogInformation("Records for customer {customerId} have been deleted successfully", customerId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}