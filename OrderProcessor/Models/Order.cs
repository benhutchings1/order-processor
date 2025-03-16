using System.ComponentModel.DataAnnotations;

namespace OrderProcessor.Models
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        public Guid OrderRef { get; set; }
        public string OrderCategory { get; set; } = null!;
        public string OrderItemID { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
    public class Order
    {
        public string OrderCategory { get; set; } = null!;
        public string OrderItemID { get; set; } = null!;
        public string Address { get; set; } = null!;
    }

    public class OrderOutbox
    {
        [Key]
        public int Id { get; set; }
        public Guid OrderRef { get; set; }
        public string ItemID { get; set; } = null!;
    }
}
