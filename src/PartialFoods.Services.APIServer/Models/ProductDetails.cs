using System;
using Newtonsoft.Json;

namespace PartialFoods.Services.APIServer.Models
{
    public class ProductDetails
    {
        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("available_qty")]
        public int Quantity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}