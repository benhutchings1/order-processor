using Microsoft.EntityFrameworkCore;
using OrderProducer.Connections.Repository;

namespace OrderProcessor.Connections.Repository;

public class OrderContext : DbContext
{   
    public DbSet<OrderOutbox> Outbox { get; set; } = null!;

    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }
}