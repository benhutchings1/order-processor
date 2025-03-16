using Microsoft.EntityFrameworkCore;
using OrderProcessor.Connections.Repository;
using OrderProducer.Connections.Repository;

namespace OrderProcessor.Processing
{
    public class Producer
    {
        OrderContext _db;
        IMessageBroker _messageBroker;
        public Producer(OrderContext db, IMessageBroker messageBroker)
        {
            _db = db;
            _messageBroker = messageBroker;
        }
        public async Task ForeverFetchAndProcessOrders(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                List<OrderOutbox> outbox = await _db.Outbox.ToListAsync();
                if (outbox.Count() > 0)
                {
                    Console.WriteLine($"Adding {outbox.Count()} messages...");
                    await _messageBroker.SendOrders(outbox);
                    Console.WriteLine($"...Added {outbox.Count()} messages");
                    await _db.Outbox.Where(x => outbox.Select(x => x.Id).Contains(x.Id)).ExecuteDeleteAsync();
                    Console.WriteLine("Deleted messages from outbox");
                }
                await Task.Delay(3000);
            }
            _messageBroker.Dispose();
        }


    }
}
