using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PartialFoods.Services.APIServer.Models
{
    public class NewOrderRequest
    {
        [JsonProperty("created_on")]
        public long CreatedOn { get; set; }
        [JsonProperty("tax_rate")]
        public int TaxRate { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("line_items")]
        public ICollection<LineItem> LineItems { get; set; }

        public class LineItem
        {
            [JsonProperty("sku")]
            public string SKU { get; set; }
            [JsonProperty("quantity")]
            public int Quantity { get; set; }
        }
    }
}

