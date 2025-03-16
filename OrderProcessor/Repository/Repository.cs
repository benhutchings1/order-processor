using Microsoft.EntityFrameworkCore;
using OrderProcessor.Models;

namespace OrderProcessor.Repository;

public class OrderContext : DbContext
{
    public DbSet<OrderEntity> Order { get; set; } = null!;
    public DbSet<OrderOutbox> Outbox { get; set; } = null!;

    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }
}