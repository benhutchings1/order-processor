using OrderProcessor.Models;
using OrderProcessor.Repository;

namespace OrderProcessor.Orders
{
    public interface IOrderService
    {
        Task<OrderEntity> SubmitOrder(Order newOrder);
    }
    public class OrderService : IOrderService
    {
        OrderContext _db;
        public OrderService(OrderContext db)
        {
            _db = db;
        }

        public async Task<OrderEntity> SubmitOrder(Order newOrder)
        {
            Guid orderGuid = Guid.NewGuid();
            OrderEntity newOrderEntity = new OrderEntity
            {
                OrderRef = orderGuid,
                OrderCategory = newOrder.OrderCategory,
                OrderItemID = newOrder.OrderItemID,
                Address  = newOrder.Address
            };
            await SubmitOrderToTable(newOrderEntity);

            return newOrderEntity;
        }

        private async Task SubmitOrderToTable(OrderEntity newOrder)
        {
            OrderOutbox newOrderOutbox = new OrderOutbox
            {
                OrderRef = newOrder.OrderRef,
                ItemID = newOrder.OrderItemID
            };
            _db.Order.Add(newOrder);
            _db.Outbox.Add(newOrderOutbox);
            await _db.SaveChangesAsync();
        }
    }
}
