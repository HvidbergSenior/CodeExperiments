using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Product
{
    public class BusinessCentralProduct : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("No")]
        public string? Number { get; set; }
        [JsonPropertyName("Description")]
        public string? Description { get; set; }
        [JsonPropertyName("GenProdPostingGroup")]
        public string? GenProdPostingGroup { get; set; }
        [JsonPropertyName("BaseUnitOfMeasure")]
        public string? BaseUnitOfMeasure { get; set; }
        [JsonPropertyName("ItemCategoryCode")]
        public string? ItemCategoryCode { get; set; }
        [JsonPropertyName("InventoryPostingGroup")]
        public string? InventoryPostingGroup { get; set; }
        [JsonPropertyName("Densitet_Netweight")]
        public decimal DensitetNetweight { get; set; }
        [JsonPropertyName("Inventory_Posting_Group_desc")]
        public string? InventoryPostingGroupDesc { get; set; }
        [JsonPropertyName("Gen_Prod_Post_Grp_desc")]
        public string? GenProdPostGrpDesc { get; set; }

        public Guid SystemId { get; set; } // Todo: Move to base type when we confirm they all have it mapped.
    }
}