using Newtonsoft.Json;

namespace cosmodb.Model;

public class CosmosDocumentBase
{
    [JsonProperty(PropertyName = "id")] public string Id { get; set; }

    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; }
}