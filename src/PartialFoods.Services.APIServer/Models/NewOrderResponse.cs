using Newtonsoft.Json;

namespace PartialFoods.Services.APIServer.Models
{
    public class NewOrderResponse
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }
    }
}