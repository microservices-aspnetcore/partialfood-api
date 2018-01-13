using Microsoft.AspNetCore.Mvc;
using PartialFoods.Services.APIServer.Models;

namespace PartialFoods.Services.APIServer.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private InventoryManagement.InventoryManagementClient inventoryManagementClient;

        public ProductsController(InventoryManagement.InventoryManagementClient invClient)
        {
            this.inventoryManagementClient = invClient;
        }

        [HttpGet("{sku}")]
        public ProductDetails GetProduct(string sku)
        {
            var qty = inventoryManagementClient.GetEffectiveQuantity(new GetQuantityRequest { SKU = sku.ToUpper() });

            return new ProductDetails
            {
                SKU = sku,
                Quantity = (int)qty.Quantity,
                Name = "TBD",
                Description = "TBD"
            };
        }
    }
}