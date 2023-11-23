namespace EldudkaServer.Models.DTO
{
    public class OrderProductDTO
    {
        public int OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Int16 Amount { get; set; }
    }
}
