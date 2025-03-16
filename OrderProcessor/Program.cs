using Microsoft.EntityFrameworkCore;
using OrderProcessor.Orders;
using OrderProcessor.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add DI
builder.Services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("db") ?? throw new ArgumentNullException()
));
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
