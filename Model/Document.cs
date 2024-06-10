using Newtonsoft.Json;

namespace cosmodb.Model;

public class Document : CosmosDocumentBase
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>
    /// The customer identifier.
    /// </value>
    [JsonProperty("customerId")]
    public string CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>
    /// The date.
    /// </value>
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the list of categories.
    /// </summary>
    /// <value>
    /// The categories.
    /// </value>
    [JsonProperty("categories")]
    public List<string> Categories { get; set; }

    /// <summary>
    /// Gets or sets the list of products.
    /// </summary>
    /// <value>
    /// The products.
    /// </value>
    [JsonProperty("products")]
    public List<string> Products { get; set; }

    /// <summary>
    /// Gets or sets type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    [JsonProperty("tags")]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Gets or sets the document identifier.
    /// </summary>
    /// <value>
    /// The document identifier.
    /// </value>
    [JsonProperty("docId")]
    public string DocId { get; set; }

    /// <summary>
    /// Gets or sets the reference.
    /// </summary>
    /// <value>
    /// The external reference.
    /// </value>
    [JsonProperty("reference")]
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the .
    /// </summary>
    /// <value>
    ///   <c>true</c> if purged; otherwise, <c>false</c>.
    /// </value>
    [JsonProperty("purged")]
    public bool Purged { get; set; }

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    /// <value>
    /// The account number.
    /// </value>
    [JsonProperty("accountNumber")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>
    /// The source.
    /// </value>
    [JsonProperty("source")]
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the file extension.
    /// </summary>
    /// <value>
    /// The file extension.
    /// </value>
    [JsonProperty("fileExtension")]
    public string FileExtension { get; set; } = "application/pdf";

    /// <summary>
    /// Gets or sets the .
    /// </summary>
    /// <value>
    /// The documento is invoice
    /// </value>
    [JsonProperty("isInvoice")]
    public bool IsInvoice { get; set; }

    /// <summary>
    /// Gets or sets the purged fileName.
    /// </summary>
    /// <value>
    /// The purged fileName.
    /// </value>
    [JsonProperty("fileNamePt")]
    public string FileNamePt { get; set; }

    /// <summary>
    /// Gets or sets the purged fileName.
    /// </summary>
    /// <value>
    /// The purged fileName.
    /// </value>
    [JsonProperty("fileNameEn")]
    public string FileNameEn { get; set; }
}