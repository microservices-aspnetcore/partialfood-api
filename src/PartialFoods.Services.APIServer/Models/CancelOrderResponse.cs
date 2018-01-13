using Newtonsoft.Json;

namespace PartialFoods.Services.APIServer.Models
{
    public class CancelOrderResponse
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }

        [JsonProperty("canceled")]
        public bool Canceled { get; set; }

        [JsonProperty("confirmation_code")]
        public string ConfirmationCode { get; set; }
    }
}