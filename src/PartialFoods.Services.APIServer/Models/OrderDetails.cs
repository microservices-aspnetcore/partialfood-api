using System.Collections.Generic;
using Newtonsoft.Json;

namespace PartialFoods.Services.APIServer.Models
{
    public class OrderDetails
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }

        [JsonProperty("created_on")]
        public ulong CreatedOn { get; set; }

        [JsonProperty("tax_rate")]
        public ulong TaxRate { get; set; }

        [JsonProperty("line_items")]
        public ICollection<OrderItem> LineItems { get; set; } = new List<OrderItem>();

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class OrderItem
    {
        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("quantity")]
        public uint Quantity { get; set; }

        [JsonProperty("unit_price")]
        public uint UnitPrice { get; set; }
    }
}