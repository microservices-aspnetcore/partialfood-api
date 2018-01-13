using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PartialFoods.Services.APIServer.Models;
using System;
using Microsoft.Extensions.Logging;

namespace PartialFoods.Services.APIServer.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private OrderManagement.OrderManagementClient orderManagementClient;
        private OrderCommand.OrderCommandClient orderCommandClient;
        private ILogger<OrdersController> logger;

        public OrdersController(OrderManagement.OrderManagementClient client,
        OrderCommand.OrderCommandClient orderCommandClient,
        ILogger<OrdersController> logger) : base()
        {
            this.orderManagementClient = client;
            this.orderCommandClient = orderCommandClient;
            this.logger = logger;

            logger.LogInformation("Created Orders Controller");
        }

        [HttpPost]
        public NewOrderResponse CreateOrder([FromBody]NewOrderRequest newOrder)
        {
            logger.LogInformation("Accepting new order for user {0}", newOrder.UserID);
            var req = new OrderRequest
            {
                CreatedOn = (ulong)DateTime.UtcNow.Ticks,
                UserID = newOrder.UserID,
                TaxRate = (uint)newOrder.TaxRate,
                ShippingInfo = new ShippingInfo(), // ignoring this detail for now

            };
            foreach (var li in newOrder.LineItems)
            {
                req.LineItems.Add(new LineItem
                {
                    SKU = li.SKU,
                    Quantity = (uint)li.Quantity
                });
            }
            var result = orderCommandClient.SubmitOrder(req);
            return new NewOrderResponse
            {
                Accepted = result.Accepted,
                OrderID = result.OrderID
            };
        }

        [HttpDelete("{orderid}")]
        public CancelOrderResponse CancelOrder(string orderid)
        {
            // TODO : don't hardcode the user ID. Ideally this would come from a propogated bearer token
            // exposed via middleware
            var result = orderCommandClient.CancelOrder(new CancelOrderRequest { OrderID = orderid, UserID = "kevin" });
            return new CancelOrderResponse
            {
                OrderID = result.OrderID,
                Canceled = result.Canceled,
                ConfirmationCode = result.ConfirmationCode
            };
        }

        [HttpGet("{orderid}")]
        public OrderDetails GetOrder(string orderid)
        {
            var internalOrder = orderManagementClient.GetOrder(new GetOrderRequest { OrderID = orderid });

            OrderDetails response = new OrderDetails
            {
                OrderID = internalOrder.OrderID,
                CreatedOn = internalOrder.CreatedOn,
                TaxRate = internalOrder.TaxRate,
                Status = "open",
                LineItems = (
                    from li in internalOrder.LineItems
                    select new OrderItem
                    {
                        SKU = li.SKU,
                        Quantity = li.Quantity,
                        UnitPrice = li.UnitPrice
                    }
                ).ToArray()
            };

            if (internalOrder.Status == OrderStatus.Canceled)
            {
                response.Status = "canceled";
            }

            return response;
        }
    }
}