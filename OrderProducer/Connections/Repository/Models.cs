using System.ComponentModel.DataAnnotations;


namespace OrderProducer.Connections.Repository
{
    public class OrderOutbox
    {
        [Key]
        public int Id { get; set; }
        public Guid OrderRef { get; set; }
        public string ItemID { get; set; } = null!;
    }
}
